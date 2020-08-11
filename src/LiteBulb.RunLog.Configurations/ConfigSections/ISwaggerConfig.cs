
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
		//Info Info { get; set; }

		string Title { get; set; }
		string Description { get; set; }
		string Version { get; set; }
	}
}
