using LiteBulb.Common.DataModel;

namespace LiteBulb.MemoryDb
{
	public interface IMemoryDatabase
	{
		/// <summary>
		/// Name of database.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Creates a collection in the database.
		/// </summary>
		/// <typeparam name="TDocument">Type of document object for this collection</typeparam>
		/// <param name="collectionName">Name of collection to create</param>
		void CreateCollection<TDocument>(string collectionName) where TDocument : BaseModel<int>; //TODO: I don't like this here

		/// <summary>
		/// Gets a collection from the database.
		/// </summary>
		/// <typeparam name="TDocument">Type of document object for this collection</typeparam>
		/// <param name="collectionName">Name of collection to create</param>
		IMemoryCollection<TDocument> GetCollection<TDocument>(string collectionName) where TDocument : BaseModel<int>; //TODO: I don't like this here

		/// <summary>
		/// Checks if a colletion exists.
		/// </summary>
		/// <param name="collectionName">Name of the collection to check</param>
		/// <returns>Boolean of whether collection exists or not</returns>
		bool DoesCollectionExist(string collectionName);

		/// <summary>
		/// Drops a collection from the database.
		/// NOTE: not used.
		/// </summary>
		/// <param name="collectionName">Name of collection to drop</param>
		void DropCollection(string collectionName);
	}
}