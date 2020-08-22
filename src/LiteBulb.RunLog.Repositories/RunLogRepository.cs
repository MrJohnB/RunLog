using LiteBulb.Common.DataModel;
using LiteBulb.MemoryDb;
using LiteBulb.MemoryDb.Enumerations;
using LiteBulb.Repositories.MemoryDb;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LiteBulb.RunLog.Repositories
{
	/// <summary>
	/// Abstract base class for all RunLog repositories.
	/// </summary>
	/// <typeparam name="TModel">Type of the Model managed by the repository. It has to be a class.</typeparam>
	public abstract class RunLogRepository<TModel> : Repository<TModel, DatabaseContext>, IRunLogRepository<TModel> where TModel : BaseModel<int>
	{
		protected RunLogRepository(DatabaseContext databaseContext, string collectionName) : base(databaseContext, collectionName)
		{
		}

		public virtual IEnumerable<TModel> GetAll(SortDirection sortDirection = SortDirection.Descending)
		{
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

		/// <summary>
		/// Delete all objects (entities or documents) from the database collection (i.e. reset collection).
		/// </summary>
		/// <returns>Count documents deleted from the collection</returns>
		public virtual int RemoveAll()
		{
			return Collection.DeleteAll();
		}

		public virtual int Remove(Func<TModel, bool> filter)
		{
			return Collection.DeleteMany(filter);
		}

		/// <summary>
		/// Get paged list of objects (entities or documents) from the RunLog database collection.
		/// </summary>
		/// <param name="offset">(optional: omit if default values are acceptable)</param>
		/// <param name="limit">(optional: omit if default values are acceptable)</param>
		/// <returns>Paged collection of the RunLog database collection</returns>
		public virtual IPagedResult<TModel> GetPagedList(int offset = 0, int limit = 50)
		{
			// Get total record/object count
			var totalRecordCount = Collection.Count();

			var models = Collection.FindAll(SortDirection.Descending, offset, limit); //TODO: could this return null?

			if (!models.Any())
			{
				return new PagedResult<TModel>()
				{
					Data = new TModel[0],
					Total = totalRecordCount
				};
			}

			return new PagedResult<TModel>()
			{
				Data = (TModel[])models,
				Total = totalRecordCount
			};
		}
	}
}