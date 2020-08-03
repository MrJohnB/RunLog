using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using LiteBulb.RunLog.Web.ConfigSections;

namespace LiteBulb.RunLog.Web
{
	public class Program
	{
		/// <summary>
		/// Configuration property for RunLog Web Client.
		/// </summary>
		protected IConfiguration Configuration { get; }

		public static async Task Main(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("app");

			//builder.Services.AddSingleton<IApiSettings>(builder.Configuration.GetSection(nameof(ApiSettings)).Get<ApiSettings>());
			builder.Services.AddSingleton(sp => builder.Configuration.GetSection(nameof(ApiSettings)).Get<ApiSettings>());

			//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
			//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]) });
			builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(sp.GetRequiredService<ApiSettings>().BaseUrl) });

			//builder.Services.AddSingleton<LocationService>();
			//builder.Services.AddScoped<LocationService>();

			await builder.Build().RunAsync();
		}
	}
}
