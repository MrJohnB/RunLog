
namespace LiteBulb.RunLog.Api.LoadTests.Constants
{
	/// <summary>
	/// Constant string values that could be in a config file, but are in this class here for now.
	/// </summary>
	public class ConfigSettings
	{
		/// <summary>
		/// Base URL for REST API (includes port).
		/// </summary>
		public const string BaseUrl = "https://localhost:44375";

		/// <summary>
		/// Base request URI for REST API (includes version number).
		/// </summary>
		public const string BaseRequestUri = "/api/v1";

		/// <summary>
		/// URI of Activities controller.
		/// </summary>
		public const string ActivitiesRequestUri = "/activities";

		/// <summary>
		/// URI of Positions controller.
		/// </summary>
		public const string PositionsRequestUri = "/positions";
	}
}
