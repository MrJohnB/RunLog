using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace LiteBulb.RunLog.Api.LoadTests.ApiClients
{
	public interface IRestHttpClient
	{
		/// <summary>
		/// Make a GET HTTP request by calling HttpClient.GetAsync().
		/// </summary>
		/// <param name="requestUri">The Uri the request is sent to</param>
		/// <param name="configRequestHeaders">HttpRequestHeaders Action (null by default)</param>
		/// <returns>HttpResponseMessage response</returns>
		Task<HttpResponseMessage> GetAsync(string requestUri, Action<HttpRequestHeaders> configRequestHeaders = null);

		/// <summary>
		/// Make a POST HTTP request by calling HttpClient.PostAsync().
		/// </summary>
		/// <param name="requestUri">The Uri the request is sent to</param>
		/// <param name="payload">The payload plain string to send in the request body</param>
		/// <param name="configRequestHeaders">HttpRequestHeaders Action (null by default)</param>
		/// <returns>HttpResponseMessage response</returns>
		Task<HttpResponseMessage> PostAsync(string requestUri, string payload, Action<HttpRequestHeaders> configRequestHeaders = null);

		/// <summary>
		/// Make a POST HTTP request by calling HttpClient.PostAsync().
		/// </summary>
		/// <param name="requestUri">The Uri the request is sent to</param>
		/// <param name="payload">The payload object (DTO/ViewModel?) to serialize and send in the request body</param>
		/// <param name="configRequestHeaders">HttpRequestHeaders Action (null by default)</param>
		/// <returns>HttpResponseMessage response</returns>
		Task<HttpResponseMessage> PostAsync(string requestUri, object payload, Action<HttpRequestHeaders> configRequestHeaders = null);

		/// <summary>
		/// Make a PUT HTTP request by calling HttpClient.PutAsync().
		/// </summary>
		/// <param name="requestUri">The Uri the request is sent to</param>
		/// <param name="payload">The payload object (DTO/ViewModel?) to serialize and send in the request body</param>
		/// <param name="configRequestHeaders">HttpRequestHeaders Action (null by default)</param>
		/// <returns>HttpResponseMessage response</returns>
		Task<HttpResponseMessage> PutAsync(string requestUri, object payload, Action<HttpRequestHeaders> configRequestHeaders = null);

		/// <summary>
		/// Make a DELETE HTTP request by calling HttpClient.DeleteAsync().
		/// </summary>
		/// <param name="requestUri">The Uri the request is sent to</param>
		/// <param name="configRequestHeaders">HttpRequestHeaders Action (null by default)</param>
		/// <returns>HttpResponseMessage response</returns>
		Task<HttpResponseMessage> DeleteAsync(string requestUri, Action<HttpRequestHeaders> configRequestHeaders = null);
	}
}