
namespace LiteBulb.MemoryDb.LoadTests.Constants
{
	/// <summary>
	/// Constant string values that could be in a config file, but are in this class here for now.
	/// </summary>
	public class ConfigSettings
	{
		/// <summary>
		/// Database connection string.
		/// Note: MemoryDb does not require a connection string at this time.
		/// </summary>
		public const string ConnectionString = "";

		/// <summary>
		/// Name of database for this application.
		/// </summary>
		public const string DatabaseName = "RunLogDb";

		/// <summary>
		/// Name of collection for Activity object.
		/// </summary>
		public const string ActivityCollectionName = "Activity";

		/// <summary>
		/// Name of collection for Position object.
		/// </summary>
		public const string PositionCollectionName = "Position";
	}
}
