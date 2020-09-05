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
		/// Allows (from this class) setting the Enabled property on the ITransactionLogger instance.
		/// </summary>
		/// <param name="enabled">Whether transaction logging should be enabled</param>
		void SetTransactionLoggingEnablement(bool enabled);

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