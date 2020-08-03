using LiteBulb.RunLog.Models;
using LiteBulb.RunLog.Repositories.Activities;
using LiteBulb.RunLog.Services.Responses;
using System;
using System.Collections.Generic;

namespace LiteBulb.RunLog.Services.Activities
{
	/// <summary>
	/// Service for database operations (CRUD) for Position object (POCO).
	/// </summary>
	public class PositionService : IPositionService
	{
		private readonly IPositionRepository _repository;

		/// <summary>
		/// Constructor for service for database CRUD operations on Position objects.
		/// </summary>
		/// <param name="repository">IPositionRepository instance</param>
		public PositionService(IPositionRepository repository)
		{
			_repository = repository;
		}

		/// <summary>
		/// Get list of objects (entities or documents) from the Position collection.
		/// </summary>
		/// <returns>Collection of Position objects</returns>
		public ServiceResponse<IEnumerable<Position>> GetList()
		{
			var result = _repository.GetAll();

			if (result == null)
			{
				return new ServiceResponse<IEnumerable<Position>>(true,
					"Error occurred while retrieving paged list of Position objects.  Result was null for some reason.");
			}

			return new ServiceResponse<IEnumerable<Position>>(result);
		}

		/// <summary>
		/// Get a Position object by Id.
		/// </summary>
		/// <param name="id">Position id</param>
		/// <returns>Position object</returns>
		public ServiceResponse<Position> GetById(int id)
		{
			var result = _repository.GetById(id);

			if (result == null)
			{
				return new ServiceResponse<Position>(true,
					$"Error occurred while retrieving Position object with id '{id}'.  Position object was not found in the database.");
			}

			return new ServiceResponse<Position>(result);
		}

		/// <summary>
		/// Add a new Position object to the database.
		/// </summary>
		/// <param name="position">Position object to be added</param>
		/// <returns>The Position object after it was added (with the Position Id)</returns>
		public ServiceResponse<Position> Add(Position position)
		{
			Position result;

			try
			{
				result = _repository.Add(position);
			}
			catch (Exception ex)
			{
				return new ServiceResponse<Position>(true,
					$"Error occurred while adding Position object to database.  {ex.GetType()} was thrown.  Exception Message: {ex.Message}", ex);
			}

			if (result == null)
			{
				return new ServiceResponse<Position>(true,
					"Error occurred while adding Position object to database.  Position object returned by add process was null for some reason.");
			}

			return new ServiceResponse<Position>(result);
		}

		/// <summary>
		/// Update a Position POCO object.
		/// </summary>
		/// <param name="position">Position object to be updated</param>
		/// <returns>The Position object after it was updated (as returned from MemoryDb Update method)</returns>
		public ServiceResponse<Position> Update(Position position)
		{
			Position updatedResult = null;

			try
			{
				updatedResult = _repository.Update(position);
			}
			//catch (NotFoundException ex)
			//{
			//	return new ServiceResponse<Position>(true,
			//		$"Error occurred while updating a Position object.  Position with id: '{position.Id}' was NOT found.  Update was not successful.  Exception follows: {ex.Message}");
			//}
			catch (Exception ex)
			{
				return new ServiceResponse<Position>(true,
					$"Error occurred while updating a Position object.  Update was not successful.  Exception follows: {ex.Message}");
			}

			if (updatedResult == null)
			{
				return new ServiceResponse<Position>(true,
					"Error occurred while updating a Position object.  Update returned null for some reason.");
			}

			if (updatedResult == default(Position))
			{
				return new ServiceResponse<Position>(true,
					$"Error occurred while updating a Position object.  Position with id: '{position.Id}' was NOT found.  Update was not successful.");
			}

			return new ServiceResponse<Position>(updatedResult);
		}

		/// <summary>
		/// Delete a Position object/document by Id.
		/// </summary>
		/// <param name="id">Position id</param>
		/// <returns>Boolean of whether delete was successful or not</returns>
		public ServiceResponse<bool> RemoveById(int id)
		{
			var deleteSuccessful = _repository.RemoveById(id);

			if (!deleteSuccessful)
			{
				return new ServiceResponse<bool>(true,
					$"Error occurred while deleting Position object with id '{id}'.  Position object was not found in the database.");
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
					$"Error occurred while attempting to delete all Position objects from the database.  Some or all Position objects may not have been deleted.");
			}

			return new ServiceResponse<int>(deletedCount);
		}
	}
}