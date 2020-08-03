using LiteBulb.Common.Serializers;
using System;
using System.Threading.Tasks;

namespace LiteBulb.RunLog.Api.LoadTests.ApiClients
{
	/// <summary>
	/// A wrapper around the LiteBulb.RunLog.Api.LoadTests.ApiClients.RestHttpClient class.
	/// Note: reason for this wrapper is to deserialize HttpResponseMessage into objects to return.
	/// </summary>
	public class ApiClient : IApiClient
	{
		private readonly IRestHttpClient _restHttpClient;
		private readonly ISerializer _serializer;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="restHttpClient">IRestHttpClient instance</param>
		/// <param name="serializer">ISerializer instance</param>
		public ApiClient(IRestHttpClient restHttpClient, ISerializer serializer)
		{
			_restHttpClient = restHttpClient;
			_serializer = serializer;
		}

		/// <summary>
		/// Call API to create a new object.
		/// </summary>
		/// <typeparam name="T">Object type for payload sent and return type</typeparam>
		/// <param name="requestUri">URI of the controller and rest of route</param>
		/// <param name="payload">Object instance to serialze and send</param>
		/// <returns>Object that was created by the API (includes the Id field value)</returns>
		public async Task<T> CreateAsync<T>(string requestUri, T payload)
		{
			var httpResponseMessage = await _restHttpClient.PostAsync(requestUri, payload);

			if (!httpResponseMessage.IsSuccessStatusCode)
				throw new Exception($"The POST HTTP response was not successful.  Status code: '{httpResponseMessage.StatusCode}'.  Reason phrase: '{httpResponseMessage.ReasonPhrase}'.");

			string content = await httpResponseMessage.Content.ReadAsStringAsync();
			return _serializer.Deserialize<T>(content);
		}

		/// <summary>
		/// Call API to get an object by id, or get a list of all objects.
		/// </summary>
		/// <typeparam name="T">Type of object to get</typeparam>
		/// <param name="requestUri">URI of the controller and rest of route (could also include object id if single item is desired)</param>
		/// <returns>Object from the API</returns>
		public async Task<T> ReadAsync<T>(string requestUri)
		{
			var httpResponseMessage = await _restHttpClient.GetAsync(requestUri);

			if (!httpResponseMessage.IsSuccessStatusCode)
			{
				if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
					return default;

				throw new Exception($"The GET HTTP response was not successful.  Status code: '{httpResponseMessage.StatusCode}'.  Reason phrase: '{httpResponseMessage.ReasonPhrase}'.");
			}

			string content = await httpResponseMessage.Content.ReadAsStringAsync();
			return _serializer.Deserialize<T>(content);
		}

		/// <summary>
		/// Call API to update an object by id.
		/// </summary>
		/// <typeparam name="T">Type of object to update</typeparam>
		/// <param name="requestUri">URI of the controller and rest of route (which includes the object id)</param>
		/// <returns>Object from the API after it was updated</returns>
		public async Task<T> UpdateAsync<T>(string requestUri, T payload)
		{
			var httpResponseMessage = await _restHttpClient.PutAsync(requestUri, payload);

			if (!httpResponseMessage.IsSuccessStatusCode)
				throw new Exception($"The PUT HTTP response was not successful.  Status code: '{httpResponseMessage.StatusCode}'.  Reason phrase: '{httpResponseMessage.ReasonPhrase}'.");

			string content = await httpResponseMessage.Content.ReadAsStringAsync();
			return _serializer.Deserialize<T>(content);
		}

		/// <summary>
		/// Call API to delete an object by id.
		/// </summary>
		/// <typeparam name="T">Type of object to delete</typeparam>
		/// <param name="requestUri">URI of the controller and rest of route (which includes the object id)</param>
		/// <returns>String response from the API after delete</returns>
		public async Task<string> DeleteAsync(string requestUri)
		{
			var httpResponseMessage = await _restHttpClient.DeleteAsync(requestUri);

			if (!httpResponseMessage.IsSuccessStatusCode)
				throw new Exception($"The DELETE HTTP response was not successful.  Status code: '{httpResponseMessage.StatusCode}'.  Reason phrase: '{httpResponseMessage.ReasonPhrase}'.");

			string content = await httpResponseMessage.Content.ReadAsStringAsync();
			return content;
		}
	}
}
