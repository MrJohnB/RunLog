namespace LiteBulb.RunLog.Web.ConfigSections
{
	public interface IApiSettings
	{
		/// <summary>
		/// Base URL for REST API (includes port).
		/// </summary>
		string BaseUrl { get; set; }

		/// <summary>
		/// Base request URI for REST API (includes version number).
		/// </summary>
		string BaseRequestUri { get; set; }

		/// <summary>
		/// URI of Activities controller.
		/// </summary>
		string ActivitiesRequestUri { get; set; }

		/// <summary>
		/// URI of Positions controller.
		/// </summary>
		string PositionsRequestUri { get; set; }
	}
}