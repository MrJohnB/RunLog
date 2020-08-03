using LiteBulb.Repositories;
using LiteBulb.RunLog.Models;

namespace LiteBulb.RunLog.Repositories.Activities
{
	/// <summary>
	/// Repository interface definition for database operations (CRUD) for Activity object.
	/// Note: Defines the Model class used for this repository instance and the model Id data type (TModel and TId for generics in base class).
	/// </summary>
	public interface IPositionRepository
		: IRepository<Position, int>,
		IRunLogRepository<Position>
	{
	}
}
