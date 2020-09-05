using LiteBulb.MemoryDb;
using System.IO;

namespace LiteBulb.RunLog.Services.Database
{
	/// <summary>
	/// Manager class responsible for restoring database data.
	/// </summary>
	public static class DatabaseRestoreManager
	{
		/// <summary>
		/// Restore data to MemoryDb from transaction log file.
		/// </summary>
		/// <param name="databasecontext">DatabaseContext instance</param>
		/// <param name="filePath">Path to database transaction log file</param>
		public static void Restore(DatabaseContext databasecontext, string filePath)
		{
			if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
				return;

			var restoreService = new DatabaseRestoreService(databasecontext);
			restoreService.RestoreFromTransactionLog(filePath);
		}
	}
}
