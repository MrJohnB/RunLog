using System;
using System.Collections.Generic;

namespace LiteBulb.RunLog.Repositories
{
	public interface IRunLogRepository<TModel>
	{
		IEnumerable<TModel> GetAll();
		IEnumerable<TModel> Get(Func<TModel, bool> filter);
		IEnumerable<TModel> Update(Func<TModel, bool> filter, TModel model);
		int RemoveAll();
		int Remove(Func<TModel, bool> filter);
	}
}