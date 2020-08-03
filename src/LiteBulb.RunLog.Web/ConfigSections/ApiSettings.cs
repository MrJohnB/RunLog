
namespace LiteBulb.RunLog.Web.ConfigSections
{
	/// <summary>
	/// URL values for the REST API.
	/// </summary>
	public class ApiSettings : IApiSettings
	{
		/// <summary>
		/// Base URL for REST API (includes port).
		/// </summary>
		public string BaseUrl { get; set; }

		/// <summary>
		/// Base request URI for REST API (includes version number).
		/// </summary>
		public string BaseRequestUri { get; set; }

		/// <summary>
		/// URI of Activities controller.
		/// </summary>
		public string ActivitiesRequestUri { get; set; }

		/// <summary>
		/// URI of Positions controller.
		/// </summary>
		public string PositionsRequestUri { get; set; }
	}
}
