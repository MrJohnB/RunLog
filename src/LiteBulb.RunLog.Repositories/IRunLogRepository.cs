using LiteBulb.MemoryDb.Enumerations;
using System;
using System.Collections.Generic;

namespace LiteBulb.RunLog.Repositories
{
	public interface IRunLogRepository<TModel>
	{
		IEnumerable<TModel> GetAll(SortDirection sortDirection = SortDirection.Descending);
		IEnumerable<TModel> Get(Func<TModel, bool> filter, SortDirection sortDirection = SortDirection.Descending);
		IEnumerable<TModel> Update(Func<TModel, bool> filter, TModel model);
		int RemoveAll();
		int Remove(Func<TModel, bool> filter);
	}
}