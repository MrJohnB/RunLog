using LiteBulb.MemoryDb;
using LiteBulb.RunLog.Models;

namespace LiteBulb.RunLog.Repositories.Activities
{
	/// <summary>
	/// Repository class definition for database operations (CRUD) for Activity object.
	/// Note: Defines the Model class used for this repository instance and the model Id data type (TModel and TId for generics in base class).
	/// </summary>
	public class PositionRepository
		: RunLogRepository<Position>,
		IPositionRepository
	{
		public PositionRepository(DatabaseContext databaseContext, string collectionName)
			: base(databaseContext, collectionName)
		{
		}
	}
}
