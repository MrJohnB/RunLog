
namespace LiteBulb.RunLog.Configurations.ConfigSections
{
	/// <summary>
	/// Interface for Swagger configuration class.
	/// </summary>
	public interface ISwaggerConfig
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
