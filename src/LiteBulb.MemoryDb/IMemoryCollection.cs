using LiteBulb.MemoryDb.Enumerations;
using System;
using System.Collections.Generic;

namespace LiteBulb.MemoryDb
{
	/// <summary>
	/// Class representing a collection of documents in the database (AKA a "table").
	/// </summary>
	/// <typeparam name="TDocument">Type of object that will be stored as the document</typeparam>
	public interface IMemoryCollection<TDocument>
	{
		/// <summary>
		/// Name of the collection (AKA "table" name).
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Count of all documents in the collection.
		/// </summary>
		int Count();

		/// <summary>
		/// Count documents in the collection with a search filter.
		/// </summary>
		/// <param name="filter">Filter to use to search collection</param>
		/// <returns>Number of documents matching the given search terms</returns>
		int Count(Func<TDocument, bool> filter);

		/// <summary>
		/// Get full list of documents currently in the collection.
		/// </summary>
		/// <param name="sortDirection">Specifies the sort order (ascending by default)</param>
		/// <param name="offset">(optional: omit if default values are acceptable)</param>
		/// <param name="limit">(optional: omit if default values are acceptable)</param>
		/// <returns>Collection of documents</returns>
		IEnumerable<TDocument> FindAll(SortDirection sortDirection = SortDirection.Ascending, int offset = 0, int limit = int.MaxValue);

		/// <summary>
		/// Find a document in the collection by id.
		/// </summary>
		/// <param name="id">Document id</param>
		/// <returns>Document object with the given id</returns>
		TDocument Find(int id);

		/// <summary>
		/// Find one or more documents in the collection with a search filter.
		/// </summary>
		/// <param name="filter">Filter to use to search collection</param>
		/// <param name="sortDirection">Specifies the sort order (ascending by default)</param>
		/// <returns>Collection of documents matching the given search terms</returns>
		IEnumerable<TDocument> FindMany(Func<TDocument, bool> filter, SortDirection sortDirection = SortDirection.Ascending);

		/// <summary>
		/// Insert a new document into the collection.
		/// </summary>
		/// <param name="document">Document to insert</param>
		/// <returns>Document object after it was inserted (includes Id value assigned by database)</returns>
		TDocument Insert(TDocument document);

		/// <summary>
		/// Insert a collection of documents at once.
		/// </summary>
		/// <param name="documents">Collection of documents to insert</param>
		/// <returns>Collection of documents after they were inserted (includes Id values assigned by database)</returns>
		IEnumerable<TDocument> InsertMany(IEnumerable<TDocument> documents);

		/// <summary>
		/// Update a document in the collection by Id.
		/// </summary>
		/// <param name="document">Document with values to update with (must contain the Id value for the document to be updated)</param>
		/// <returns>Document object after it was updated</returns>
		TDocument Update(TDocument document);

		/// <summary>
		/// Update many documents in the collection at once with a search filter.
		/// </summary>
		/// <param name="filter">The filter to search for documents to update</param>
		/// <param name="document">Document with values to update all filtered items with (all documents that match the search term will be update with the same values)</param>
		/// <returns>Collection of one or more documents that were updated with each document Id</returns>
		IEnumerable<TDocument> UpdateMany(Func<TDocument, bool> filter, TDocument document);

		/// <summary>
		/// Delete all documents in the collection.
		/// </summary>
		/// <returns>Count of documents that were deleted</returns>
		int DeleteAll();

		/// <summary>
		/// Delete single document by id.
		/// </summary>
		/// <param name="id">Document id</param>
		/// <returns>Boolean of true or false whether delete was successful</returns>
		bool Delete(int id);

		/// <summary>
		/// Delete one or more documents based on a search filter.
		/// </summary>
		/// <param name="filter">The filter to search for documents to delete</param>
		/// <returns>Count of documents that were deleted (matched the search filter)</returns>
		int DeleteMany(Func<TDocument, bool> filter);

		/// <summary>
		/// Returns a queryable source of documents.
		/// TODO: implement here, or put this in an extension method?
		/// </summary>
		/// <returns>A queryable source of documents</returns>
		//System.Linq.IQueryable<TDocument> AsQueryable();
	}
}