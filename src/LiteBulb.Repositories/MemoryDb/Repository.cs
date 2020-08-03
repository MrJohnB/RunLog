using LiteBulb.Common.DataModel;
using LiteBulb.MemoryDb;
using System;
using System.Collections.Generic;

namespace LiteBulb.Repositories.MemoryDb
{
	/// <summary>
	/// Provides implementation for base repository operations.  Implementation of CRUD database operations.
	/// This class is abstract and cannot be instantiated directly. 
	/// </summary>
	/// <typeparam name="TModel">Type of the Model managed by the repository (it has to be a class)</typeparam>
	/// <typeparam name="TDatabaseContext">Type of DatabaseContext representing the database (designed as a substiute for DbContext)</typeparam>
	public abstract class Repository<TModel, TDatabaseContext> : BaseRepository<TDatabaseContext>, IRepository<TModel, int> // IRepository<TModel, TId>
		where TModel : BaseModel<int>
		where TDatabaseContext : DatabaseContext
	{
		protected IMemoryCollection<TModel> Collection { get; }

		/// <summary>
		/// Protected constructor for Repositor class.  Creates a new instance of the class.
		/// </summary>
		/// <param name="databaseContext">TDatabaseContext instance</param>
		/// <param name="collectionName">Name of database collection</param>
		protected Repository(TDatabaseContext databaseContext, string collectionName) : base(databaseContext)
		{
			Collection = databaseContext.GetCollection<TModel>(collectionName);
		}

		/// <summary>
		/// Adds a new model instance to the repository.
		/// This method is virtual and when needed can be overridden in derived classes.
		/// </summary>
		/// <param name="model">Model to be added</param>
		/// <returns>Model instance updated after added to the repository</returns>
		public virtual TModel Add(TModel model)
		{
			if (model == null)
				throw new ArgumentNullException();

			return Collection.Insert(model);
		}

		/// <summary>
		/// Adds a range of new models to the repository.
		/// </summary>
		/// <param name="models">Models to be added</param>
		/// <returns>Collection of Model instances updated after added to the repository</returns>
		public virtual IEnumerable<TModel> AddRange(IEnumerable<TModel> models)
		{
			if (models == null)
				throw new ArgumentNullException();

			return Collection.InsertMany(models);
		}

		/// <summary>
		/// Returns the model instance based on its Id.
		/// </summary>
		/// <param name="id">Id value of the model to be returned</param>
		/// <returns>Model instance</returns>
		public virtual TModel GetById(int id)
		{
			//int intId = Convert.ToInt32(id);

			TModel model = Collection.Find(id);

			if (model == default(TModel))
				return null;

			return model;
		}

		/// <summary>
		/// Updates the model instance.
		/// </summary>
		/// <param name="model">Model to be updated</param>
		/// <returns>Updated version of the model</returns>
		public virtual TModel Update(TModel model)
		{
			if (model == null)
				throw new ArgumentNullException();

			TModel updatedModel = Collection.Update(model);

			if (updatedModel == default(TModel))
				return null;

			return updatedModel;
		}

		/// <summary>
		/// Removes the Model from the repository based on its Id.
		/// </summary>
		/// <param name="id">Value of the Model's Id property</param>
		/// <returns>True if any model is removed, otherwise false</returns>
		public virtual bool RemoveById(int id)
		{
			//int intId = Convert.ToInt32(id);

			return Collection.Delete(id);
		}

		/// <summary>
		/// Removes the Model from the repository.
		/// </summary>
		/// <param name="model">Model to be removed</param>
		public virtual bool Remove(TModel model)
		{
			if (model == null)
				throw new ArgumentNullException();

			//int intId = Convert.ToInt32(model.Id);

			return Collection.Delete(model.Id);
		}
	}
}
