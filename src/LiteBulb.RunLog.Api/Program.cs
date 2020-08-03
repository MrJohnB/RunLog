using System;
using System.Collections.Generic;
using System.Linq;
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
	}
}
