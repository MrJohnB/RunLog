using LiteBulb.RunLog.Models;
using LiteBulb.RunLog.Services.Responses;
using System.Collections.Generic;

namespace LiteBulb.RunLog.Services.Activities
{
	/// <summary>
	/// Service interface for database operations (CRUD) for Activity object (POCO).
	/// </summary>
	public interface IActivityService
	{
		/// <summary>
		/// Get list of objects (entities or documents) from the Activity collection.
		/// Note: does not include the Position child collection for each Activity.
		/// </summary>
		/// <returns>Collection of Activity objects</returns>
		ServiceResponse<IEnumerable<Activity>> GetList();

		/// <summary>
		/// Get a Activity object by Id.
		/// </summary>
		/// <param name="id">Activity id</param>
		/// <returns>Activity object</returns>
		ServiceResponse<Activity> GetById(int id);

		/// <summary>
		/// Add a new Activity object to the database.
		/// </summary>
		/// <param name="activity">Activity object to be added</param>
		/// <returns>The Activity object after it was added (with the Activity Id)</returns>
		ServiceResponse<Activity> Add(Activity activity);

		/// <summary>
		/// Update a Activity POCO object.
		/// </summary>
		/// <param name="activity">Activity object to be updated</param>
		/// <returns>The Activity object after it was updated (as returned from MemoryDb Update method)</returns>
		ServiceResponse<Activity> Update(Activity activity);

		/// <summary>
		/// Delete a Activity object/document by Id.
		/// </summary>
		/// <param name="id">Activity id</param>
		/// <returns>Boolean of whether delete was successful or not</returns>
		ServiceResponse<bool> RemoveById(int id);

		/// <summary>
		/// Delete all documents from database (AKA reset database).
		/// </summary>
		/// <returns>Integer count of how many documents where deleted from the collection</returns>
		ServiceResponse<int> RemoveAll();
	}
}