using LiteBulb.RunLog.Configurations.ConfigSections;
using LiteBulb.MemoryDb;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
			serviceCollection.AddSingleton<DatabaseContext>(new DatabaseContext(databaseSettings.DatabaseName));

			//serviceCollection.AddSingleton<DatabaseContext>(sp => new DatabaseContext(sp.GetRequiredService<IOptions<DatabaseSettings>>().Value.ConnectionString, sp.GetRequiredService<IOptions<DatabaseSettings>>().Value.DatabaseName));
		}
	}
}
