
namespace LiteBulb.RunLog.Configurations.ConfigSections
{
	/// <summary>
	/// Implementation for Swagger configuration class.
	/// </summary>
	public class SwaggerConfig : ISwaggerConfig
	{
		/// <summary>
		/// Swagger Info class property accessor.
		/// </summary>
		//public Info Info { get; set; }

		public string Title { get; set; }
		public string Description { get; set; }
		public string Version { get; set; }
	}
}
