using System;
using Microsoft.Extensions.Configuration;

namespace LiteBulb.RunLog.Configurations.Extensions
{
	/// <summary>
	/// Extension methods for dealing with configuration files.
	/// </summary>
	public static class ConfigurationExtensions
	{
		/// <summary>
		/// Gets the configuration section in appsettings.json file and maps it to a strongly typed class object.
		/// </summary>
		/// <typeparam name="T">Type (class) of the configuration section</typeparam>
		/// <param name="configuration">IConfiguration instance</param>
		/// <returns>Stronly typed object with the config section properties mapped to it</returns>
		public static T GetSection<T>(this IConfiguration configuration)
		{
			if (configuration == null)
			{
				throw new ArgumentNullException(nameof(configuration));
			}

			IConfigurationSection configurationSection = configuration.GetSection(typeof(T).Name);
			T section = configurationSection.Get<T>();

			return section;
		}
	}
}
