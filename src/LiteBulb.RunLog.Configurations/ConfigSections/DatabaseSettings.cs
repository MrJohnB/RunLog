
namespace LiteBulb.RunLog.Configurations.ConfigSections
{
	/// <summary>
	/// Implementation of DatabaseSettings POCO class.
	/// </summary>
	public class DatabaseSettings : IDatabaseSettings
	{
		/// <summary>
		/// Database connection string.
		/// Note: MemoryDb does not require a connection string at this time.
		/// </summary>
		public string ConnectionString { get; set; }

		/// <summary>
		/// Name of database for this application.
		/// </summary>
		public string DatabaseName { get; set; }

		/// <summary>
		/// Name of collection for Activity object.
		/// </summary>
		public string ActivityCollectionName { get; set; }

		/// <summary>
		/// Name of collection for Position object.
		/// </summary>
		public string PositionCollectionName { get; set; }
	}
}
