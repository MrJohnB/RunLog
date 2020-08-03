using LiteBulb.Common.DataModel;
using System;
using System.Collections.Generic;

namespace LiteBulb.MemoryDb
{
	public class MemoryDatabase : IMemoryDatabase
	{
		/// <summary>
		/// Name of database.
		/// </summary>
		public string Name { get; }

		// Key: Name of collection
		// Value: Collection object
		private readonly Dictionary<string, object> _collections;

		// Key: Name of collection
		// Value: Type of collection
		// Note: for casting
		private readonly Dictionary<string, Type> _collectionTypes;

		/// <summary>
		/// Constructor for MemoryDatabase.
		/// </summary>
		/// <param name="databaseName">Name of database</param>
		public MemoryDatabase(string databaseName)
		{
			Name = databaseName;
			_collectionTypes = new Dictionary<string, Type>();
			_collections = new Dictionary<string, object>();
		}

		/// <summary>
		/// Creates a collection in the database.
		/// </summary>
		/// <typeparam name="TDocument">Type of document object for this collection</typeparam>
		/// <param name="collectionName">Name of collection to create</param>
		public void CreateCollection<TDocument>(string collectionName) where TDocument : BaseModel<int> //TODO: I don't like this here
		{
			if (string.IsNullOrWhiteSpace(collectionName))
				throw new ArgumentNullException();

			if (_collections.ContainsKey(collectionName) || _collectionTypes.ContainsKey(collectionName))
				throw new ArgumentException($"The database already contains a collection with name: '{collectionName}'.");

			_collectionTypes.Add(collectionName, typeof(TDocument));
			_collections.Add(collectionName, new MemoryCollection<TDocument>(collectionName));
		}

		/// <summary>
		/// Gets a collection from the database.
		/// </summary>
		/// <typeparam name="TDocument">Type of document object for this collection</typeparam>
		/// <param name="collectionName">Name of collection to create</param>
		public IMemoryCollection<TDocument> GetCollection<TDocument>(string collectionName) where TDocument : BaseModel<int> //TODO: I don't like this here
		{
			if (string.IsNullOrWhiteSpace(collectionName))
				throw new ArgumentNullException();

			if (!_collections.ContainsKey(collectionName) || !_collectionTypes.ContainsKey(collectionName))
				//throw new ArgumentException($"The database does not contain a collection with name: '{collectionName}'.");
				CreateCollection<TDocument>(collectionName); // Automatically create new collection

			var type = _collectionTypes[collectionName];

			if (type != typeof(TDocument)) // or: TDocument.GetType()
				throw new InvalidOperationException($"The generic type argument used: '{typeof(TDocument)}' does not match the actual type of the collection: '{type}'");

			return _collections[collectionName] as IMemoryCollection<TDocument>;
		}

		/// <summary>
		/// Checks if a colletion exists.
		/// </summary>
		/// <param name="collectionName">Name of the collection to check</param>
		/// <returns>Boolean of whether collection exists or not</returns>
		public bool DoesCollectionExist(string collectionName)
		{
			if (string.IsNullOrWhiteSpace(collectionName))
				throw new ArgumentNullException();

			if (_collections.ContainsKey(collectionName) && _collectionTypes.ContainsKey(collectionName))
				return true;

			return false;
		}

		/// <summary>
		/// Drops a collection from the database.
		/// NOTE: not used.
		/// </summary>
		/// <param name="collectionName">Name of collection to drop</param>
		public void DropCollection(string collectionName)
		{
			if (string.IsNullOrWhiteSpace(collectionName))
				throw new ArgumentNullException();

			if (!_collections.ContainsKey(collectionName) || !_collectionTypes.ContainsKey(collectionName))
				throw new ArgumentException($"The database does not contain a collection with name: '{collectionName}'.");

			_collectionTypes.Remove(collectionName);
			_collections.Remove(collectionName);
		}
	}
}
