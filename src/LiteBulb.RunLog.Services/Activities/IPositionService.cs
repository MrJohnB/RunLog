using LiteBulb.Common.DataModel;
using LiteBulb.RunLog.Models;
using LiteBulb.RunLog.Services.Responses;
using System.Collections.Generic;

namespace LiteBulb.RunLog.Services.Activities
{
	/// <summary>
	/// Service interface for database operations (CRUD) for Position object (POCO).
	/// </summary>
	public interface IPositionService
	{
		/// <summary>
		/// Get paged list of Position objects from the database.
		/// </summary>
		/// <param name="offset">(optional: omit if default values are acceptable)</param>
		/// <param name="limit">(optional: omit if default values are acceptable)</param>
		/// <returns>Paged collection of Position objects from the database collection</returns>
		ServiceResponse<IPagedResult<Position>> GetPagedList(int offset = 0, int limit = 50);

		/// <summary>
		/// Get list of objects (entities or documents) from the Position collection.
		/// </summary>
		/// <returns>Collection of Position objects</returns>
		ServiceResponse<IEnumerable<Position>> GetList();

		/// <summary>
		/// Get a Position object by Id.
		/// </summary>
		/// <param name="id">Position id</param>
		/// <returns>Position object</returns>
		ServiceResponse<Position> GetById(int id);

		/// <summary>
		/// Add a new Position object to the database.
		/// </summary>
		/// <param name="position">Position object to be added</param>
		/// <returns>The Position object after it was added (with the Position Id)</returns>
		ServiceResponse<Position> Add(Position position);

		/// <summary>
		/// Update a Position POCO object.
		/// </summary>
		/// <param name="position">Position object to be updated</param>
		/// <returns>The Position object after it was updated (as returned from MemoryDb Update method)</returns>
		ServiceResponse<Position> Update(Position position);

		/// <summary>
		/// Delete a Position object/document by Id.
		/// </summary>
		/// <param name="id">Position id</param>
		/// <returns>Boolean of whether delete was successful or not</returns>
		ServiceResponse<bool> RemoveById(int id);

		/// <summary>
		/// Delete all documents from database (AKA reset database).
		/// </summary>
		/// <returns>Integer count of how many documents where deleted from the collection</returns>
		ServiceResponse<int> RemoveAll();
	}
}