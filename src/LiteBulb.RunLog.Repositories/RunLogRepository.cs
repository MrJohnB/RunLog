using LiteBulb.Common.DataModel;
using LiteBulb.MemoryDb;
using LiteBulb.MemoryDb.Enumerations;
using LiteBulb.Repositories.MemoryDb;
using System;
using System.Collections.Generic;

namespace LiteBulb.RunLog.Repositories
{
	public abstract class RunLogRepository<TModel> : Repository<TModel, DatabaseContext>, IRunLogRepository<TModel> where TModel : BaseModel<int>
	{
		protected RunLogRepository(DatabaseContext databaseContext, string collectionName) : base(databaseContext, collectionName)
		{
		}

		public virtual IEnumerable<TModel> GetAll(SortDirection sortDirection = SortDirection.Descending)
		{
			//TODO: make paged
			return Collection.FindAll(sortDirection); //TODO: could this return null?
		}

		public virtual IEnumerable<TModel> Get(Func<TModel, bool> filter, SortDirection sortDirection = SortDirection.Descending)
		{
			return Collection.FindMany(filter, sortDirection);
		}

		public virtual IEnumerable<TModel> Update(Func<TModel, bool> filter, TModel model)
		{
			return Collection.UpdateMany(filter, model);
		}

		public virtual int RemoveAll()
		{
			return Collection.DeleteAll();
		}

		public virtual int Remove(Func<TModel, bool> filter)
		{
			return Collection.DeleteMany(filter);
		}
	}
}