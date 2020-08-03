
namespace LiteBulb.RunLog.Configurations.ConfigSections
{
	/// <summary>
	/// Interface for DatabaseSettings POCO class.
	/// </summary>
	public interface IDatabaseSettings
	{
		/// <summary>
		/// Database connection string.
		/// Note: MemoryDb does not require a connection string at this time.
		/// </summary>
		string ConnectionString { get; set; }

		/// <summary>
		/// Name of database for this application.
		/// </summary>
		string DatabaseName { get; set; }

		/// <summary>
		/// Name of collection for Activity object.
		/// </summary>
		string ActivityCollectionName { get; set; }

		/// <summary>
		/// Name of collection for Position object.
		/// </summary>
		string PositionCollectionName { get; set; }
	}
}
