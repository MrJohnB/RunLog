using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using LiteBulb.Common.Serializers;

namespace LiteBulb.RunLog.Api.LoadTests.ApiClients
{
	/// <summary>
	/// A wrapper around the System.Net.Http.HttpClient class.
	/// </summary>
	public class RestHttpClient : IRestHttpClient
	{
		private const string MediaType = "application/json";

		private readonly HttpClient _httpClient;
		private readonly ISerializer _serializer;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="baseUrl">Base URL for the REST API (i.e. domain name and maybe also port)</param>
		public RestHttpClient(string baseUrl)
		{
			if (string.IsNullOrWhiteSpace(baseUrl))
				throw new ArgumentException("Invalid value for service base Url.", nameof(baseUrl));

			_httpClient = new HttpClient() { BaseAddress = new Uri(baseUrl) };

			_serializer = NewtonsoftSerializer.GetInstance();
		}

		/// <summary>
		/// Make a GET HTTP request by calling HttpClient.GetAsync().
		/// </summary>
		/// <param name="requestUri">The Uri the request is sent to</param>
		/// <param name="configRequestHeaders">HttpRequestHeaders Action (null by default)</param>
		/// <returns>HttpResponseMessage response</returns>
		public async Task<HttpResponseMessage> GetAsync(string requestUri, Action<HttpRequestHeaders> configRequestHeaders = null)
		{
			configRequestHeaders?.Invoke(_httpClient.DefaultRequestHeaders);
			HttpResponseMessage httpResponse = await _httpClient.GetAsync(requestUri);
			ClearHeaders();
			return httpResponse;
		}

		/// <summary>
		/// Make a POST HTTP request by calling HttpClient.PostAsync().
		/// </summary>
		/// <param name="requestUri">The Uri the request is sent to</param>
		/// <param name="payload">The payload plain string to send in the request body</param>
		/// <param name="configRequestHeaders">HttpRequestHeaders Action (null by default)</param>
		/// <returns>HttpResponseMessage response</returns>
		public async Task<HttpResponseMessage> PostAsync(string requestUri, string payload, Action<HttpRequestHeaders> configRequestHeaders = null)
		{
			ClearHeaders();
			configRequestHeaders?.Invoke(_httpClient.DefaultRequestHeaders);
			HttpResponseMessage httpResponse = await _httpClient.PostAsync(requestUri, CreateStringContent(payload));
			ClearHeaders();
			return httpResponse;
		}

		/// <summary>
		/// Make a POST HTTP request by calling HttpClient.PostAsync().
		/// </summary>
		/// <param name="requestUri">The Uri the request is sent to</param>
		/// <param name="payload">The payload object (DTO/ViewModel?) to serialize and send in the request body</param>
		/// <param name="configRequestHeaders">HttpRequestHeaders Action (null by default)</param>
		/// <returns>HttpResponseMessage response</returns>
		public async Task<HttpResponseMessage> PostAsync(string requestUri, object payload, Action<HttpRequestHeaders> configRequestHeaders = null)
		{
			ClearHeaders();
			configRequestHeaders?.Invoke(_httpClient.DefaultRequestHeaders);
			string content = _serializer.Serialize(payload);
			HttpResponseMessage httpResponse = await _httpClient.PostAsync(requestUri, CreateStringContent(content));
			ClearHeaders();
			return httpResponse;
		}

		/// <summary>
		/// POST request wrapper method.
		/// </summary>
		/// <typeparam name="T">DTO object type</typeparam>
		/// <param name="requestUri">API resource URI</param>
		/// <param name="payload">Payload or request/resource body (usually JSON and usually a DTO)</param>
		/// <returns>The IRestResponse<T> response from a HttpResponseMessage (from a HttpClient request)</returns>
		//public static async Task<T> PostRequestAsync<T>(string requestUri, object payload)
		//{
		//	return await GetResultAsync(async () => await RestClient.PostAsync<T>(requestUri, payload));
		//}

		/// <summary>
		/// Make a PUT HTTP request by calling HttpClient.PutAsync().
		/// </summary>
		/// <param name="requestUri">The Uri the request is sent to</param>
		/// <param name="payload">The payload object (DTO/ViewModel?) to serialize and send in the request body</param>
		/// <param name="configRequestHeaders">HttpRequestHeaders Action (null by default)</param>
		/// <returns>HttpResponseMessage response</returns>
		public async Task<HttpResponseMessage> PutAsync(string requestUri, object payload, Action<HttpRequestHeaders> configRequestHeaders = null)
		{
			ClearHeaders();
			configRequestHeaders?.Invoke(_httpClient.DefaultRequestHeaders);
			string content = _serializer.Serialize(payload);
			HttpResponseMessage httpResponse = await _httpClient.PutAsync(requestUri, CreateStringContent(content));
			ClearHeaders();
			return httpResponse;
		}

		/// <summary>
		/// Make a DELETE HTTP request by calling HttpClient.DeleteAsync().
		/// </summary>
		/// <param name="requestUri">The Uri the request is sent to</param>
		/// <param name="configRequestHeaders">HttpRequestHeaders Action (null by default)</param>
		/// <returns>HttpResponseMessage response</returns>
		public async Task<HttpResponseMessage> DeleteAsync(string requestUri, Action<HttpRequestHeaders> configRequestHeaders = null)
		{
			ClearHeaders();
			configRequestHeaders?.Invoke(_httpClient.DefaultRequestHeaders);
			HttpResponseMessage httpResponse = await _httpClient.DeleteAsync(requestUri);
			ClearHeaders();
			return httpResponse;
		}

		/// <summary>
		/// Create the StringContent object to pass into HttpClient.PostAsync() method as HttpContent parameter.
		/// </summary>
		/// <param name="content">HttpContent to send in request</param>
		/// <param name="mediaType">Media type of HttpContent</param>
		/// <returns>StringContent object</returns>
		private StringContent CreateStringContent(string content, string mediaType = MediaType)
		{
			return new StringContent(content, Encoding.UTF8, mediaType);
		}

		/// <summary>
		/// Clear HttpClient default request headers.
		/// </summary>
		private void ClearHeaders()
		{
			_httpClient.DefaultRequestHeaders.Clear();
		}
	}
}
