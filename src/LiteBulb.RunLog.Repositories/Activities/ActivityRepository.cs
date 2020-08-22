using LiteBulb.MemoryDb;
using LiteBulb.MemoryDb.Enumerations;
using LiteBulb.RunLog.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LiteBulb.RunLog.Repositories.Activities
{
	/// <summary>
	/// Repository class definition for database operations (CRUD) for Activity object.
	/// Note: Defines the Model class used for this repository instance and the model Id data type (TModel and TId for generics in base class).
	/// </summary>
	public class ActivityRepository
		: RunLogRepository<Activity>,
		IActivityRepository
	{
		private readonly IMemoryCollection<Position> _positionCollection;

		public ActivityRepository(DatabaseContext databaseContext, string collectionName, string positionCollectionName)
			: base(databaseContext, collectionName)
		{
			_positionCollection = DatabaseContext.GetCollection<Position>(positionCollectionName);
		}

		/// <summary>
		/// Adds a new Activity object and Position objects (if present) to the repository.
		/// TODO: https://stackoverflow.com/questions/10901020/which-is-faster-clear-collection-or-instantiate-new
		/// </summary>
		/// <param name="activity">Activity to be added</param>
		/// <returns>Model instance updated after added to the repository</returns>
		public override Activity Add(Activity activity)
		{
			if (activity == null)
				throw new ArgumentNullException();

			var positions = activity.Positions;
			activity.Positions = new List<Position>();
			activity.PositionCount = null;

			var insertedActivity = Collection.Insert(activity);
			var insertedPositions = new List<Position>();

			foreach (var position in positions)
			{
				position.ActivityId = insertedActivity.Id; // Get the Activity Id
				var insertedPosition = _positionCollection.Insert(position);
				insertedPositions.Add(insertedPosition);
			}

			insertedActivity.Positions = insertedPositions;
			
			return insertedActivity;
		}

		/// <summary>
		/// Adds range of new Activity objects and Position objects (if present) to the repository.
		/// </summary>
		/// <param name="models">Models to be added</param>
		/// <returns>Collection of Model instances updated after added to the repository</returns>
		public override IEnumerable<Activity> AddRange(IEnumerable<Activity> activities)
		{
			if (activities == null)
				throw new ArgumentNullException();

			var insertedActivities = new List<Activity>();

			foreach (var activity in activities)
			{
				var insertedActivity = Add(activity);
				insertedActivities.Add(insertedActivity);
			}

			return insertedActivities;
		}

		/// <summary>
		/// Updates the Activity object (but doesn't touch any Position objects at all).
		/// </summary>
		/// <param name="model">Model to be updated</param>
		/// <returns>Updated version of the model</returns>
		public override Activity Update(Activity model)
		{
			if (model == null)
				throw new ArgumentNullException();

			model.Positions.Clear(); // Ignore Position children
			//TODO: make a DTO that doesn't accept Position children
			//TODO: refactor update method so any items in the Positions collection get added as NEW position objects in the Position Collection
			//TODO: make it so the return object of this method includes any children contained in the Position collection?

			var updatedModel = Collection.Update(model);

			if (updatedModel == default(Activity))
				return null;

			return updatedModel;
		}

		/// <summary>
		/// Updates the Activity object (but doesn't touch any Position objects at all).
		/// </summary>
		/// <param name="filter">The filter to search for documents to update</param>
		/// <param name="model">Model with values to update all filtered items with (all models that match the search term will be update with the same values)</param>
		/// <returns>Collection of one or more models that were updated with each model Id</returns>
		public override IEnumerable<Activity> Update(Func<Activity, bool> filter, Activity model)
		{
			model.Positions.Clear(); // Ignore Position children
			//TODO: make a DTO that doesn't accept Position children
			//TODO: refactor update method so any items in the Positions collection get added as NEW position objects in the Position Collection
			//TODO: make it so the return object of this method includes any children contained in the Position collection?

			return Collection.UpdateMany(filter, model);
		}

		/// <summary>
		/// Delete single model object by id (and deletes all Position objects that belong to it).
		/// </summary>
		/// <param name="id">Model id</param>
		/// <returns>True if delete successul otherwise false</returns>
		public override bool RemoveById(int id)
		{
			_positionCollection.DeleteMany(x => x.ActivityId == id);

			return Collection.Delete(id);
		}

		/// <summary>
		/// Delete single model object by id (and deletes all Position objects that belong to it).
		/// </summary>
		/// <param name="model">Model object</param>
		/// <returns>True if delete successul otherwise false</returns>
		public override bool Remove(Activity model)
		{
			if (model == null)
				throw new ArgumentNullException();

			return RemoveById(model.Id);
		}

		/// <summary>
		/// Delete all Activity objects in the collection (and deletes all Position objects in that collection).
		///TODO: should Position objects be counted also?
		/// </summary>
		/// <returns>Count of Activity objects that were deleted (Position objects not counted)</returns>
		public override int RemoveAll()
		{
			_positionCollection.DeleteAll();
			return Collection.DeleteAll();
		}

		/// <summary>
		/// Delete one or more Activity objects based on a search filter (and deletes all Position objects that belong to them).
		/// NOTE: Position objects not counted.
		/// </summary>
		/// <param name="filter">The filter to search for Activity objects to delete</param>
		/// <returns>Count of Activity objects that were deleted (matched the search filter)</returns>
		public override int Remove(Func<Activity, bool> filter)
		{
			var activities = Collection.FindMany(filter);

			foreach (var activity in activities)
				_positionCollection.DeleteMany(x => x.ActivityId == activity.Id);

			return Collection.DeleteMany(filter);
		}

		/// <summary>
		/// Returns all Activity objects along with the count of Position objects that belong to them.
		/// Note: does not map Position child collection to parent Activity.
		/// </summary>
		/// <param name="sortDirection">Specifies the sort order (decending by default)</param>
		/// <returns>Collection of Activity objects with their corresponding mapped Position objects</returns>
		public override IEnumerable<Activity> GetAll(SortDirection sortDirection = SortDirection.Descending)
		{
			var activities = base.GetAll(sortDirection);

			if (activities == null) //TODO: this should never occur
				return null;

			foreach (var activity in activities)
				activity.PositionCount = _positionCollection.Count(x => x.ActivityId == activity.Id);

			return activities;
		}

		/// <summary>
		/// Find single Activity object along with the Position objects that belong to it.
		/// </summary>
		/// <param name="id">Activity id</param>
		/// <returns>Activity object with mapped Position objects</returns>
		public Activity GetMapped(int id)
		{
			var activity = GetById(id);

			if (activity == null)
				return null;

			MapPositionChildren(activity);
			activity.PositionCount = activity.Positions.Count;

			return activity;
		}

		/// <summary>
		/// Returns all Activity objects along with the Position objects that belong to them.
		/// </summary>
		/// <param name="sortDirection">Specifies the sort order (decending by default)</param>
		/// <returns>Collection of Activity objects with their corresponding mapped Position objects</returns>
		public IEnumerable<Activity> GetAllMapped(SortDirection sortDirection = SortDirection.Descending)
		{
			var activities = base.GetAll(sortDirection).ToArray(); //TODO: why ToArray() needed?

			if (activities == null) //TODO: this should never occur
				return null;

			foreach (var activity in activities)
			{
				MapPositionChildren(activity);
				activity.PositionCount = activity.Positions.Count;
			}

			return activities;
		}

		/// <summary>
		/// Find the Position objects that belong to the Activity and map them to child collection.
		/// </summary>
		/// <param name="sortDirection">Specifies the sort order (decending by default)</param>
		/// <param name="activity">The Activity object to map Position objects to</param>
		private void MapPositionChildren(Activity activity, SortDirection sortDirection = SortDirection.Descending)
		{
			var positions = _positionCollection.FindMany(x => x.ActivityId == activity.Id, sortDirection);
			activity.Positions.AddRange(positions);
		}
	}
}
