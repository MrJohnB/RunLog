using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using LiteBulb.MemoryDb;
using LiteBulb.RunLog.Configurations.ConfigSections;
using LiteBulb.RunLog.Configurations.Extensions;
using LiteBulb.RunLog.Repositories.Activities;
using LiteBulb.RunLog.Services.Activities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace LiteBulb.RunLog.Api
{
	/// <summary>
	/// Startup class method for GuitarTuner API.
	/// </summary>
	public class Startup
	{
		private readonly ISwaggerConfig _swaggerConfig;

		/// <summary>
		/// Startup constructor for RunLog API.
		/// </summary>
		/// <param name="configuration">IConfiguration</param>
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;

			// Get settings from config file
			_swaggerConfig = Configuration.GetSection<SwaggerConfig>();
		}

		/// <summary>
		/// Configuration property for RunLog API.
		/// </summary>
		public IConfiguration Configuration { get; }

		/// <summary>
		/// This method gets called by the runtime. Use this method to add services to the container.
		/// </summary>
		/// <param name="services">IServiceCollection instance</param>
		public void ConfigureServices(IServiceCollection services)
		{
			// Get settings from config file
			//var databaseSettings = Configuration.GetSection<DatabaseSettings>();
			//TODO: Might not need this here..
			services.Configure<DatabaseSettings>(
				Configuration.GetSection(nameof(DatabaseSettings)));

			// Need this for config settings that get dependency injected
			//TODO: Don't need this here..
			services.AddSingleton<IDatabaseSettings>(sp =>
				sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);

			// Add database context
			services.AddDataContext(Configuration);

			// Add application services
			//services.AddSingleton<IActivityRepository, ActivityRepository>();
			services.AddSingleton<IActivityRepository>(sp =>
				new ActivityRepository(
					sp.GetRequiredService<DatabaseContext>(),
					sp.GetRequiredService<IOptions<DatabaseSettings>>().Value.ActivityCollectionName, // "Activity object"
					sp.GetRequiredService<IOptions<DatabaseSettings>>().Value.PositionCollectionName)); // "Position object"
			services.AddSingleton<IActivityService, ActivityService>();

			services.AddSingleton<IPositionRepository>(sp =>
				new PositionRepository(
					sp.GetRequiredService<DatabaseContext>(),
					sp.GetRequiredService<IOptions<DatabaseSettings>>().Value.PositionCollectionName)); // "Position object"
			services.AddSingleton<IPositionService, PositionService>();

			//services.AddCors(options =>
			//{
			//	options.AddDefaultPolicy(
			//		builder =>
			//		{
			//			builder.WithOrigins("http://example1.com", "http://www.example2.com");
			//			//builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
			//		});
			//});

			// Add Json settings
			//services.AddControllers();
			services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false) // ASP.NET Core 3.0 bug: https://stackoverflow.com/questions/59288259/asp-net-core-3-0-createdataction-returns-no-route-matches-the-supplied-values
				.AddNewtonsoftJson(options => options.UseMemberCasing()); // Probably don't need this (so don't need Microsoft.AspNetCore.Mvc.NewtonsoftJson) because I'm not using model annotations

			// Register the Swagger generator, defining 1 or more Swagger documents
			// https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle
			services.AddSwaggerGen(o =>
			{
				o.SwaggerDoc(_swaggerConfig.Version, new OpenApiInfo { Title = _swaggerConfig.Title, Version = _swaggerConfig.Version, Description = _swaggerConfig.Description });
				o.IncludeXmlComments(GetXmlPath());
				//o.DescribeAllEnumsAsStrings(); // Comment out if you want to input int values instead of string values
			});
			//services.AddSwaggerGenNewtonsoftSupport(); // explicit opt-in - needs to be placed after AddSwaggerGen() https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/1269
		}

		/// <summary>
		/// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		/// </summary>
		/// <param name="app">IApplicationBuilder instance</param>
		/// <param name="env">IWebHostEnvironment instance</param>
		/// <param name="logger">ILogger instance</param>
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
		{
			// Logging stuff

			// The following will be picked up by Application Insights.
			logger.LogInformation("Logging from ConfigureServices.");

			// Logging: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/
			logger.LogTrace("Trace");
			logger.LogDebug("Debug");
			logger.LogInformation("Information");
			logger.LogWarning("Warning");
			logger.LogError("Error");
			logger.LogCritical("Critical");

			// Verbose: Verbose is the noisiest level, rarely (if ever) enabled for a production app.
			// Debug: Debug is used for internal system events that are not necessarily observable from the outside, but useful when determining how something happened.
			// Information: Information events describe things happening in the system that correspond to its responsibilities and functions.Generally these are the observable actions the system can perform.
			// Warning: When service is degraded, endangered, or may be behaving outside of its expected parameters, Warning level events are used.
			// Error: When functionality is unavailable or expectations broken, an Error event is used.
			// Fatal: The most critical level, Fatal events demand immediate attention.

			// Middleware stuff

			// Enable middleware to serve generated Swagger as a JSON endpoint.
			app.UseSwagger();

			var swaggerEndpoint = $"/swagger/{_swaggerConfig.Version}/swagger.json";

			// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint(swaggerEndpoint, _swaggerConfig.Title);
				// To serve the Swagger UI at the app's root (http://localhost:<port>/), set the RoutePrefix property to an empty string:
				c.RoutePrefix = string.Empty;
			});

			if (env.IsDevelopment())
			{
				logger.LogInformation("In Development environment");
				app.UseDeveloperExceptionPage();
			}
			//else
			//{
			//	app.UseExceptionHandler("/Error");
			//	app.UseHsts();
			//}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseCors(options => options // Call to UseCors() must be placed after UseRouting, but before UseAuthorization
				.AllowAnyOrigin()
				.AllowAnyHeader()
				.AllowAnyMethod());

			// app.UseResponseCaching();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}

		/// <summary>
		/// Set the comments path for the Swagger JSON and UI.
		/// </summary>
		/// <returns>Path of the swagger.json file</returns>
		private static string GetXmlPath()
		{
			string basePath = AppContext.BaseDirectory;
			string assemblyName = Assembly.GetEntryAssembly().GetName().Name;
			string fileName = Path.GetFileName(assemblyName + ".xml");
			return Path.Combine(basePath, fileName);
		}
	}
}
