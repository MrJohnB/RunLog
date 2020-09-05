
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

		/// <summary>
		/// Whether to log database transactions to Debug window.
		/// </summary>
		public bool LogToDebug { get; set; }

		/// <summary>
		/// Whether to log database transactions to file (JSON).
		/// </summary>
		public bool LogToFile { get; set; }

		/// <summary>
		/// File path for the database transaction log.
		/// </summary>
		public string TransactionLogFilePath { get; set; }
	}
}
