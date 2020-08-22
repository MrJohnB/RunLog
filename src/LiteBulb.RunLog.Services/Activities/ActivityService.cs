using LiteBulb.RunLog.Models;
using LiteBulb.RunLog.Repositories.Activities;
using LiteBulb.RunLog.Services.Responses;
using System;
using System.Collections.Generic;

namespace LiteBulb.RunLog.Services.Activities
{
	/// <summary>
	/// Service for database operations (CRUD) for Activity object (POCO).
	/// </summary>
	public class ActivityService : IActivityService
	{
		private readonly IActivityRepository _repository;

		/// <summary>
		/// Constructor for service for database CRUD operations on Activity objects.
		/// </summary>
		/// <param name="repository">IActivityRepository instance</param>
		public ActivityService(IActivityRepository repository)
		{
			_repository = repository;
		}

		/// <summary>
		/// Get list of objects (entities or documents) from the Activity collection.
		/// Note: does not include the Position child collection for each Activity.
		/// </summary>
		/// <returns>Collection of Activity objects</returns>
		public ServiceResponse<IEnumerable<Activity>> GetList()
		{
			var result = _repository.GetAll(); // GetAllMapped()

			if (result == null) //TODO: this should never occur
			{
				return new ServiceResponse<IEnumerable<Activity>>(true,
					"Error occurred while retrieving paged list of Activity objects.  Result was null for some reason.");
			}

			return new ServiceResponse<IEnumerable<Activity>>(result);
		}

		/// <summary>
		/// Get a Activity object by Id.
		/// </summary>
		/// <param name="id">Activity id</param>
		/// <returns>Activity object</returns>
		public ServiceResponse<Activity> GetById(int id)
		{
			var result = _repository.GetMapped(id);

			if (result == null)
			{
				return new ServiceResponse<Activity>(true,
					$"Error occurred while retrieving Activity object with id '{id}'.  Activity object was not found in the database.");
			}

			return new ServiceResponse<Activity>(result);
		}

		/// <summary>
		/// Add a new Activity object to the database.
		/// </summary>
		/// <param name="activity">Activity object to be added</param>
		/// <returns>The Activity object after it was added (with the Activity Id)</returns>
		public ServiceResponse<Activity> Add(Activity activity)
		{
			Activity result;

			try
			{
				result = _repository.Add(activity);
			}
			catch (Exception ex)
			{
				return new ServiceResponse<Activity>(true,
					$"Error occurred while adding an Activity object to database.  {ex.GetType()} was thrown.  Exception Message: {ex.Message}", ex);
			}

			if (result == null)
			{
				return new ServiceResponse<Activity>(true,
					"Error occurred while adding an Activity object to database.  Activity object returned by add process was null for some reason.");
			}

			return new ServiceResponse<Activity>(result);
		}

		/// <summary>
		/// Update a Activity POCO object.
		/// </summary>
		/// <param name="activity">Activity object to be updated</param>
		/// <returns>The Activity object after it was updated (as returned from MemoryDb Update method)</returns>
		public ServiceResponse<Activity> Update(Activity activity)
		{
			Activity updatedResult;

			try
			{
				updatedResult = _repository.Update(activity);
			}
			//catch (NotFoundException ex)
			//{
			//	return new ServiceResponse<Activity>(true,
			//		$"Error occurred while updating an Activity object.  Activity with id: '{activity.Id}' was NOT found.  Update was not successful.  Exception follows: {ex.Message}");
			//}
			catch (Exception ex)
			{
				return new ServiceResponse<Activity>(true,
					$"Error occurred while updating an Activity object.  Update was not successful.  Exception follows: {ex.Message}");
			}

			if (updatedResult == null)
			{
				return new ServiceResponse<Activity>(true,
					"Error occurred while updating an Activity object.  Update returned null for some reason.");
			}

			if (updatedResult == default(Activity))
			{
				return new ServiceResponse<Activity>(true,
					$"Error occurred while updating an Activity object.  Activity with id: '{activity.Id}' was NOT found.  Update was not successful.");
			}

			return new ServiceResponse<Activity>(updatedResult);
		}

		/// <summary>
		/// Delete a Activity object/document by Id.
		/// </summary>
		/// <param name="id">Activity id</param>
		/// <returns>Boolean of whether delete was successful or not</returns>
		public ServiceResponse<bool> RemoveById(int id)
		{
			var deleteSuccessful = _repository.RemoveById(id);

			if (!deleteSuccessful)
			{
				return new ServiceResponse<bool>(true,
					$"Error occurred while deleting Activity object with id '{id}'.  Activity object was not found in the database.");
			}

			return new ServiceResponse<bool>(deleteSuccessful);
		}

		/// <summary>
		/// Delete all documents from database (AKA reset database).
		/// </summary>
		/// <returns>Integer count of how many documents where deleted from the collection</returns>
		public ServiceResponse<int> RemoveAll()
		{
			var deletedCount = _repository.RemoveAll();

			//TODO: re-think RemoveAll() error handling
			if (deletedCount <= 0)
			{
				return new ServiceResponse<int>(true,
					$"Error occurred while attempting to delete all Activity objects from the database.  Some or all Activity objects may not have been deleted.");
			}

			return new ServiceResponse<int>(deletedCount);
		}
	}
}