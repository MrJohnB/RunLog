using LiteBulb.Common.DataModel;
using LiteBulb.MemoryDb.Enumerations;
using System;
using System.Collections.Generic;

namespace LiteBulb.RunLog.Repositories
{
	/// <summary>
	/// Interface for all RunLog repositories.
	/// </summary>
	/// <typeparam name="TModel">Type of the Model managed by the repository. It has to be a class.</typeparam>
	public interface IRunLogRepository<TModel>
	{
		IEnumerable<TModel> GetAll(SortDirection sortDirection = SortDirection.Descending);
		IEnumerable<TModel> Get(Func<TModel, bool> filter, SortDirection sortDirection = SortDirection.Descending);
		IEnumerable<TModel> Update(Func<TModel, bool> filter, TModel model);

		/// <summary>
		/// Delete all objects (entities or documents) from the database collection (i.e. reset collection).
		/// </summary>
		/// <returns>Count documents deleted from the collection</returns>
		int RemoveAll();
		int Remove(Func<TModel, bool> filter);

		/// <summary>
		/// Get paged list of objects (entities or documents) from the RunLog database collection.
		/// </summary>
		/// <param name="offset">(optional: omit if default values are acceptable)</param>
		/// <param name="limit">(optional: omit if default values are acceptable)</param>
		/// <returns>Paged collection of the RunLog database collection</returns>
		IPagedResult<TModel> GetPagedList(int offset = 0, int limit = 50);
	}
}