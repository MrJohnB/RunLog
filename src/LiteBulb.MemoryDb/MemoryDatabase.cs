using LiteBulb.Common.DataModel;
using LiteBulb.MemoryDb.Logging;
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

		/// <summary>
		/// Allows (from this class) setting the Enabled property on the ITransactionLogger instance.
		/// </summary>
		/// <param name="enabled">Whether transaction logging should be enabled</param>
		public void SetTransactionLoggingEnablement(bool enabled)
		{
			foreach (var transactionLogger in _transactionLoggers)
				transactionLogger.Enabled = enabled;
		}

		// Key: Name of collection
		// Value: Collection object
		private readonly Dictionary<string, object> _collections;

		// Key: Name of collection
		// Value: Type of collection
		// Note: for casting
		private readonly Dictionary<string, Type> _collectionTypes;

		// Transaction logger instances for this database (i.e. FileTransactionLogger, DebugTransactionLogger)
		private readonly IList<ITransactionLogger> _transactionLoggers;

		/// <summary>
		/// Constructor for MemoryDatabase.
		/// </summary>
		/// <param name="databaseName">Name of database</param>
		public MemoryDatabase(string databaseName)
		{
			Name = databaseName ?? throw new ArgumentNullException();
			_collectionTypes = new Dictionary<string, Type>();
			_collections = new Dictionary<string, object>();
			_transactionLoggers = new List<ITransactionLogger>();
		}

		public MemoryDatabase(string databaseName, IEnumerable<ITransactionLogger> transactionLoggers) : this(databaseName)
		{
			foreach (var transactionLogger in transactionLoggers)
				_transactionLoggers.Add(transactionLogger);
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
			var memoryCollection = new MemoryCollection<TDocument>(collectionName);
			_collections.Add(collectionName, memoryCollection);

			// Add EventHandler method for each TransactionLogger instance
			foreach (var transactionLogger in _transactionLoggers)
				memoryCollection.TransactionOccurred += transactionLogger.OnTransactionOccurred;
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
		/// Checks if a collection exists.
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
