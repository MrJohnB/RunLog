using System.Threading.Tasks;

namespace LiteBulb.RunLog.Api.LoadTests.ApiClients
{
	/// <summary>
	/// Interface for a wrapper around the LiteBulb.RunLog.Api.LoadTests.ApiClients.RestHttpClient class.
	/// Note: reason for this wrapper is to deserialize HttpResponseMessage into objects to return.
	/// </summary>
	public interface IApiClient
	{
		/// <summary>
		/// Call API to create a new object.
		/// </summary>
		/// <typeparam name="T">Object type for payload sent and return type</typeparam>
		/// <param name="requestUri">URI of the controller and rest of route</param>
		/// <param name="payload">Object instance to serialze and send</param>
		/// <returns>Object that was created by the API (includes the Id field value)</returns>
		Task<T> CreateAsync<T>(string requestUri, T payload);

		/// <summary>
		/// Call API to get an object by id, or get a list of all objects.
		/// </summary>
		/// <typeparam name="T">Type of object to get</typeparam>
		/// <param name="requestUri">URI of the controller and rest of route (could also include object id if single item is desired)</param>
		/// <returns>Object from the API</returns>
		Task<T> ReadAsync<T>(string requestUri);

		/// <summary>
		/// Call API to update an object by id.
		/// </summary>
		/// <typeparam name="T">Type of object to update</typeparam>
		/// <param name="requestUri">URI of the controller and rest of route (which includes the object id)</param>
		/// <returns>Object from the API after it was updated</returns>
		Task<T> UpdateAsync<T>(string requestUri, T payload);

		/// <summary>
		/// Call API to delete an object by id.
		/// </summary>
		/// <typeparam name="T">Type of object to delete</typeparam>
		/// <param name="requestUri">URI of the controller and rest of route (which includes the object id)</param>
		/// <returns>String response from the API after delete</returns>
		Task<string> DeleteAsync(string requestUri);
	}
}