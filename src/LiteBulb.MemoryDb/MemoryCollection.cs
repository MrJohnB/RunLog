using LiteBulb.Common.DataModel;
using LiteBulb.MemoryDb.Enumerations;
using LiteBulb.MemoryDb.Events;
using LiteBulb.MemoryDb.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LiteBulb.MemoryDb
{
	/// <summary>
	/// Class representing a collection of documents in the database (AKA a "table").
	/// TODO: should this entire class be static?
	/// </summary>
	/// <typeparam name="TDocument">Type of object that will be stored as the document</typeparam>
	public class MemoryCollection<TDocument> : IMemoryCollection<TDocument> where TDocument : BaseModel<int>
	{
		/// <summary>
		/// Name of the collection (AKA "table" name).
		/// </summary>
		public string Name { get; private set; }

		// Key: Id
		// Value: Object (POCO class)
		private readonly Dictionary<int, TDocument> _items;

		// Monitor lock around operations on the Dictionary object.
		private readonly object syncLock = new object();

		private int _identity;
		private readonly int _increment;

		/// <summary>
		/// Event and EventHandler for transaction events.
		/// </summary>
		public event EventHandler<TransactionEventArgs> TransactionOccurred;
		protected virtual void OnTransactionOccurred(TransactionType transactionType, TDocument document) =>
			TransactionOccurred?.Invoke(this, new TransactionEventArgs(new Transaction(Name, transactionType, new[] { document })));
		protected virtual void OnTransactionOccurred(TransactionType transactionType, IEnumerable<TDocument> documents) =>
			TransactionOccurred?.Invoke(this, new TransactionEventArgs(new Transaction(Name, transactionType, documents)));

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">Name of the collection (AKA "table" name)</param>
		/// <param name="seed">Seed value for the Id field</param>
		/// <param name="increment">Increment value for the Id field</param>
		public MemoryCollection(string name, int seed = 1, int increment = 1)
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException();

			if (seed <= 0 || increment <= 0 || seed > int.MaxValue || increment > int.MaxValue)
				throw new ArgumentOutOfRangeException($"Database seed: '{seed}' or increment value: '{increment}' is invalid.");

			Name = name;
			_items = new Dictionary<int, TDocument>();
			_identity = seed;
			_increment = increment;
		}

		/// <summary>
		/// Get the next usable Id value for the next document stored in the database.
		/// </summary>
		/// <returns>Id value to use</returns>
		private int GetIdAndIncrement()
		{
			if (_identity >= int.MaxValue)
				throw new InvalidOperationException($"The database identity value: '{_identity}' is too large and cannot be incremented.");

			int currentIdentity = 0;

			lock (syncLock)
			{
				currentIdentity = _identity;
				_identity += _increment;
			}

			return currentIdentity;
		}

		/// <summary>
		/// Count of all documents in the collection.
		/// </summary>
		public int Count()
		{
			int count = 0;

			lock (syncLock)
			{
				count = _items.Count;
			}

			return count;
		}

		/// <summary>
		/// Count documents in the collection with a search filter.
		/// </summary>
		/// <param name="filter">Filter to use to search collection</param>
		/// <returns>Number of documents matching the given search terms</returns>
		public int Count(Func<TDocument, bool> filter)
		{
			if (filter == null)
				throw new ArgumentNullException();

			int count = 0;

			lock (syncLock)
			{
				count = _items.Values.Count(filter);
			}

			return count;
		}

		/// <summary>
		/// Get full list of documents currently in the collection.
		/// TODO: Not really necessary, just use other method i.e. Find(filter = null) ?
		/// </summary>
		/// <param name="sortDirection">Specifies the sort order (ascending by default)</param>
		/// <param name="offset">(optional: omit if default values are acceptable)</param>
		/// <param name="limit">(optional: omit if default values are acceptable)</param>
		/// <returns>Collection of documents</returns>
		public IReadOnlyCollection<TDocument> FindAll(SortDirection sortDirection = SortDirection.Ascending, int offset = 0, int limit = int.MaxValue)
		{
			var foundItems = new List<TDocument>();
			int index = 0;

			lock (syncLock)
			{
				var items = sortDirection == SortDirection.Ascending ? _items.Values : _items.Values.Reverse();
				//items = items.Skip(offset).Take(limit);

				foreach (var item in items)
				{
					if (index++ < offset)
						continue;

					foundItems.Add(item.CloneJson());

					if (foundItems.Count >= limit)
						break;
				}
			}

			return foundItems;
		}

		/// <summary>
		/// Find a document in the collection by id.
		/// </summary>
		/// <param name="id">Document id</param>
		/// <returns>Document object with the given id</returns>
		public TDocument Find(int id)
		{
			if (id <= 0 || id > int.MaxValue)
				throw new ArgumentOutOfRangeException($"Document id field: '{id}' is invalid.");

			//if (!_items.TryGetValue(id, out TDocument value))
			//	return null;

			TDocument foundItem;

			lock (syncLock)
			{
				var item = _items.GetValueOrDefault(id); // will be default(TDocument) if key not found
				foundItem = item.CloneJson();
			}

			return foundItem; 
		}

		/// <summary>
		/// Find one or more documents in the collection with a search filter.
		/// https://stackoverflow.com/questions/44761385/how-to-pass-a-predicate-as-parameter-c-sharp
		/// </summary>
		/// <param name="filter">Filter to use to search collection</param>
		/// <param name="sortDirection">Specifies the sort order (ascending by default)</param>
		/// <returns>Collection of documents matching the given search terms</returns>
		public IReadOnlyCollection<TDocument> FindMany(Func<TDocument, bool> filter, SortDirection sortDirection = SortDirection.Ascending)
		{
			if (filter == null)
				throw new ArgumentNullException();

			var foundItems = new List<TDocument>();

			lock (syncLock)
			{
				var items = sortDirection == SortDirection.Ascending ? _items.Values : _items.Values.Reverse();

				foreach (var item in items.Where(filter))
					foundItems.Add(item.CloneJson());
			}

			return foundItems;
		}

		/// <summary>
		/// Insert a new document into the collection.
		/// </summary>
		/// <param name="document">Document to insert</param>
		/// <returns>Document object after it was inserted (includes Id value assigned by database)</returns>
		public TDocument Insert(TDocument document)
		{
			if (document == null)
				throw new ArgumentNullException();

			var clone = document.CloneJson();

			clone.Id = GetIdAndIncrement();

			lock (syncLock)
			{
				if (!_items.TryAdd(clone.Id, clone))
					throw new ArgumentException($"The collection already contains an entry with the id: '{clone.Id}'.");
			}

			document.Id = clone.Id;

			OnTransactionOccurred(TransactionType.Insert, document);

			return document;
		}

		/// <summary>
		/// Insert a collection of documents at once.
		/// </summary>
		/// <param name="documents">Collection of documents to insert</param>
		/// <returns>Collection of documents after they were inserted (includes Id values assigned by database)</returns>
		public IEnumerable<TDocument> InsertMany(IEnumerable<TDocument> documents)
		{
			if (documents == null)
				throw new ArgumentNullException();

			foreach (var document in documents)
				yield return Insert(document); //TODO: is this a good idea?  (i.e. no lock - but each insert has a lock)
		}

		/// <summary>
		/// Update a document in the collection by Id.
		/// </summary>
		/// <param name="document">Document with values to update with (must contain the Id value for the document to be updated)</param>
		/// <returns>Document object after it was updated</returns>
		public TDocument Update(TDocument document)
		{
			if (document == null)
				throw new ArgumentNullException();

			if (document.Id <= 0 || document.Id > int.MaxValue)
				throw new ArgumentException($"Document id field: '{document.Id}' is invalid.");

			lock (syncLock)
			{
				if (!_items.ContainsKey(document.Id))
					return default;

				_items[document.Id] = document.CloneJson();
			}

			OnTransactionOccurred(TransactionType.Update, document);

			return document;
		}

		/// <summary>
		/// Update many documents in the collection at once with a search filter.
		/// </summary>
		/// <param name="filter">The filter to search for documents to update</param>
		/// <param name="document">Document with values to update all filtered items with (all documents that match the search term will be update with the same values)</param>
		/// <returns>Collection of one or more documents that were updated with each document Id</returns>
		public IReadOnlyCollection<TDocument> UpdateMany(Func<TDocument, bool> filter, TDocument document)
		{
			if (filter == null || document == null)
				throw new ArgumentNullException();

			var updatedItems = new List<TDocument>();

			lock (syncLock)
			{
				var keys = _items.Where(kvp => filter(kvp.Value)).Select(x => x.Key); //TODO: Does this actually work??

				foreach (var key in keys)
				{
					_items[key] = document.CloneJson(); // New copy for each item!
					var updatedItem = _items[key].CloneJson();
					updatedItems.Add(updatedItem);
				}
			}

			OnTransactionOccurred(TransactionType.Update, updatedItems);

			return updatedItems;
		}

		/// <summary>
		/// Delete all documents in the collection.
		/// </summary>
		/// <returns>Count of documents that were deleted</returns>
		public int DeleteAll()
		{
			TDocument[] items;

			lock (syncLock)
			{
				items = _items.Values.ToArray();
				_items.Clear();
			}

			OnTransactionOccurred(TransactionType.Delete, items);

			return items.Length;
		}

		/// <summary>
		/// Delete single document by id.
		/// </summary>
		/// <param name="id">Document id</param>
		/// <returns>Boolean of true or false whether delete was successful</returns>
		public bool Delete(int id)
		{
			if (id <= 0 || id > int.MaxValue)
				throw new ArgumentOutOfRangeException($"Document id field: '{id}' is invalid.");

			lock (syncLock)
			{
				if (_items.TryGetValue(id, out TDocument deletedItem))
				{
					if (_items.Remove(id))
					{
						OnTransactionOccurred(TransactionType.Delete, deletedItem);
						return true;
					}
				}
			}

			return false;
		}

		/// <summary>
		/// Delete one or more documents based on a search filter.
		/// </summary>
		/// <param name="filter">The filter to search for documents to delete</param>
		/// <returns>Count of documents that were deleted (matched the search filter)</returns>
		public int DeleteMany(Func<TDocument, bool> filter)
		{
			if (filter == null)
				throw new ArgumentNullException();

			var deletedItems = new List<TDocument>();

			lock (syncLock)
			{
				var keys = _items.Where(kvp => filter(kvp.Value)).Select(x => x.Key); // Does this actually work??

				foreach (var key in keys)
				{
					if (_items.TryGetValue(key, out TDocument deletedItem))
						if (_items.Remove(key))
							deletedItems.Add(deletedItem);
				}
			}

			OnTransactionOccurred(TransactionType.Delete, deletedItems);

			return deletedItems.Count;
		}
	}
}
