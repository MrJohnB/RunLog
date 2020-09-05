using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using LiteBulb.RunLog.Configurations.ConfigSections;
using LiteBulb.MemoryDb;
using LiteBulb.MemoryDb.Logging;

namespace LiteBulb.RunLog.Configurations.Extensions
{
	/// <summary>
	/// Extension methods for adding Database data context instance during DI Container initialization.
	/// </summary>
	public static class DataContextConfigurationExtensions
	{
		/// <summary>
		/// Adds the DatabaseContext object to the Services Collection as a singleton.
		/// </summary>
		/// <param name="serviceCollection">IServiceCollection instance</param>
		/// <param name="configuration">IConfiguration instance (with the config file)</param>
		public static void AddDataContext(this IServiceCollection serviceCollection, IConfiguration configuration)
		{
			IDatabaseSettings databaseSettings = configuration.GetSection<DatabaseSettings>();
			serviceCollection.AddSingleton<DatabaseContext>(new DatabaseContext(databaseSettings.DatabaseName, GetTransactionLoggers(databaseSettings)));

			//serviceCollection.AddSingleton<DatabaseContext>(sp => new DatabaseContext(sp.GetRequiredService<IOptions<DatabaseSettings>>().Value.ConnectionString, sp.GetRequiredService<IOptions<DatabaseSettings>>().Value.DatabaseName));
		}

		/// <summary>
		/// Create the TransactionLogger instances for use with MemoryDb (optional).
		/// </summary>
		/// <param name="databaseSettings">DatabaseSettings instance</param>
		/// <returns>TransactionLogger instance collection</returns>
		private static IEnumerable<ITransactionLogger> GetTransactionLoggers(IDatabaseSettings databaseSettings)
		{
			var transactionLoggers = new List<ITransactionLogger>();

			if (databaseSettings.LogToDebug)
				transactionLoggers.Add(new DebugTransactionLogger());

			if (databaseSettings.LogToFile && !string.IsNullOrWhiteSpace(databaseSettings.TransactionLogFilePath))
				transactionLoggers.Add(new FileTransactionLogger(databaseSettings.TransactionLogFilePath));

			return transactionLoggers;
		}
	}
}
