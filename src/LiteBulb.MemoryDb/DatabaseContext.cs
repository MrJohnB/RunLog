using LiteBulb.Common.DataModel;

namespace LiteBulb.MemoryDb
{
	/// <summary>
	/// Create singleton instance of this!
	/// </summary>
	public class DatabaseContext
	{
		//TOOD: make this support multiple databases
		public IMemoryDatabase Database { get; }

		public DatabaseContext(string databaseName)
		{
			Database = new MemoryDatabase(databaseName);
		}

		/// <summary>
		/// Gets a IMemoryCollection instance (database collection) by a collection name.
		/// </summary>
		/// <typeparam name="TDocument">Type of document used for this collection (generic)</typeparam>
		/// <param name="collectionName">Name of the collection to get</param>
		/// <returns>IMemoryCollection instance of type TDocument</returns>
		public IMemoryCollection<TDocument> GetCollection<TDocument>(string collectionName) where TDocument : BaseModel<int>
		{
			return Database.GetCollection<TDocument>(collectionName);
		}

		/// <summary>
		/// Checks if a database exists.
		/// Note: Not used.
		/// </summary>
		/// <param name="databaseName">The database name to check for</param>
		/// <returns>Boolean whether database is currently present or not</returns>
		private bool DoesDatabaseExist(string databaseName)
		{
			//TODO: loop thru collection of databases

			if (Database == null)
				return false;

			if (string.Compare(Database.Name, databaseName) != 0)
				return false;

			return true;
		}

	}
}
