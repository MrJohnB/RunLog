using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
//using Microsoft.ApplicationInsights.Extensibility;

namespace LiteBulb.RunLog.Api
{
	/// <summary>
	/// Program class for RunLog API.
	/// </summary>
	public class Program
	{
		/// <summary>
		/// Main method for RunLog API.
		/// </summary>
		/// <param name="args">Argument list passed into Main() method</param>
		public static void Main(string[] args)
		{
			ILogger<Program> logger = null;

			try
			{
				//Log.Information("Starting web host");
				var host = CreateHostBuilder(args).Build();

				// Get an instance of ILogger in ASP.NET Core 3.x
				logger = host.Services.GetRequiredService<ILogger<Program>>();
				logger.LogInformation("Starting web host");

				RestoreDatabase(host);

				host.Run();
			}
			catch (Exception ex)
			{
				//Log.Fatal(ex, "Host terminated unexpectedly");
				if (logger != null)
					logger.LogCritical(ex, "Host terminated unexpectedly");
			}
			finally
			{
				//Log.CloseAndFlush();
			}
		}

		/// <summary>
		/// CreateHostBuilder method for RunLog API.
		/// </summary>
		/// <param name="args">Arguments</param>
		/// <returns>IWebHostBuilder instance</returns>
		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureLogging(logging =>
				{
					logging.ClearProviders();
					logging.AddConsole();
					logging.AddDebug();
				})
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});

		/// <summary>
		/// Restore data to MemoryDb from transaction log file.
		/// Note: database must be restored AFTER the Repository services have been resolved (because that's when its collections get created).
		/// https://www.learnentityframeworkcore.com/migrations/seeding
		/// https://stackoverflow.com/questions/32459670/resolving-instances-with-asp-net-core-di-from-within-configureservices
		/// </summary>
		/// <param name="host">IHostBuilder instance</param>
		private static void RestoreDatabase(IHost host)
		{
			// MemoryDb and all its collections are setup now after creating Repositories above
			var databaseContext = host.Services.GetService<MemoryDb.DatabaseContext>();
			var filePath = host.Services
				.GetRequiredService<Microsoft.Extensions.Options.IOptions<Configurations.ConfigSections.DatabaseSettings>>()
				.Value.TransactionLogFilePath;
			Services.Backups.DatabaseRestoreManager.Restore(databaseContext, filePath);
		}
	}
}
