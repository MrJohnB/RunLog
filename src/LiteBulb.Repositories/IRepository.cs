using System.Collections.Generic;

namespace LiteBulb.Repositories
{
	/// <summary>
	/// Repository interface provides methods to manage object (model) persistence.  Defines required CRUD database operations.
	/// </summary>
	/// <typeparam name="TModel">Type of the Model managed by the repository (it has to be a class)</typeparam>
	/// <typeparam name="TId">Type of the property used as Id by the Model</typeparam>
	public interface IRepository<TModel, TId>
	{
		/// <summary>
		/// Adds a new model instance to the repository.
		/// This method is virtual and when needed can be overridden in derived classes.
		/// </summary>
		/// <param name="model">Model to be added</param>
		/// <returns>Model instance updated after added to the repository</returns>
		TModel Add(TModel model);

		/// <summary>
		/// Adds a range of new models to the repository.
		/// </summary>
		/// <param name="models">Models to be added</param>
		IEnumerable<TModel> AddRange(IEnumerable<TModel> models);

		/// <summary>
		/// Returns the model instance based on its Id.
		/// </summary>
		/// <param name="id">Id value of the model to be returned</param>
		/// <returns>Model instance</returns>
		TModel GetById(TId id);

		/// <summary>
		/// Updates the model instance.
		/// </summary>
		/// <param name="model">Model to be updated</param>
		/// <returns>Updated version of the model</returns>
		TModel Update(TModel model);

		/// <summary>
		/// Removes the Model from the repository based on its Id.
		/// </summary>
		/// <param name="id">Value of the Model's Id property</param>
		/// <returns>True if any model is removed, otherwise false</returns>
		bool RemoveById(TId id);

		/// <summary>
		/// Removes the Model from the repository.
		/// </summary>
		/// <param name="model">Model to be removed</param>
		bool Remove(TModel model);
	}
}