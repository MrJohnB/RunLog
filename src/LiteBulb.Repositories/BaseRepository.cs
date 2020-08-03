using LiteBulb.MemoryDb;

namespace LiteBulb.Repositories
{
	/// <summary>
	/// Base class for EntityFramework or MongoDB or MemoryDb repositories.
	/// This class is abstract and cannot be instantiated directly.
	/// </summary>
	/// <typeparam name="TDatabaseContext">Type of DbContext used by the Repository to access the database (it has to inherent from DatabaseContext)</typeparam>
	public abstract class BaseRepository<TDatabaseContext> where TDatabaseContext : DatabaseContext
	{
		protected TDatabaseContext DatabaseContext { get; }

		/// <summary>
		/// Protected constructor of BaseRepository.
		/// </summary>
		/// <param name="databaseContext">DatabaseContect instance</param>
		protected BaseRepository(TDatabaseContext databaseContext)
		{
			DatabaseContext = databaseContext;
		}
	}
}
