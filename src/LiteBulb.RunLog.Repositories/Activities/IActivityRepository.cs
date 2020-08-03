using LiteBulb.Repositories;
using LiteBulb.RunLog.Models;
using System.Collections.Generic;

namespace LiteBulb.RunLog.Repositories.Activities
{
	/// <summary>
	/// Repository interface definition for database operations (CRUD) for Activity object.
	/// Note: Defines the Model class used for this repository instance and the model Id data type (TModel and TId for generics in base class).
	/// </summary>
	public interface IActivityRepository
		: IRepository<Activity, int>,
		IRunLogRepository<Activity>
	{
		/// <summary>
		/// Find single Activity object along with the Position objects that belong to it.
		/// </summary>
		/// <param name="id">Activity id</param>
		/// <returns>Activity object with mapped Position objects</returns>
		Activity GetMapped(int id);

		/// <summary>
		/// Returns all Activity objects along with the Position objects that belong to them.
		/// </summary>
		/// <returns>Collection of Activity objects with their corresponding mapped Position objects</returns>
		IEnumerable<Activity> GetAllMapped();
	}
}
