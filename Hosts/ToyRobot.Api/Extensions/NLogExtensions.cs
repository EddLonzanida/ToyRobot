using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Config;
using NLog.Extensions.Logging;
using System;
using System.IO;
using System.Linq;

namespace ToyRobot.Api.Extensions
{
	public static class NLogExtensions
	{
		/// <summary>
		/// Setup NLog.
		/// <para>Sets the following values:</para>
		/// <code language="c#">env</code>
		/// <code language="c#">releaseVersion</code>
		/// <para>Calls <see cref="GlobalDiagnosticsContext.Set(string,object)"/> to initialize NLog variables.</para>
		/// </summary>
		public static Logger InitializeNLogAppsettings(this IConfiguration config, string env)
		{
			var assemblyVersion = UtilityExtensions.GetAssemblyVersion();

			GlobalDiagnosticsContext.Set("env", env);
			GlobalDiagnosticsContext.Set("releaseVersion", assemblyVersion);

			var logger = LogManager.Setup()
				.LoadNLogConfigurationFromAppSettings(config)
				.GetCurrentClassLogger();

			return logger;
		}

		/// <summary>
		/// Loads NLog LoggingConfiguration from appsettings.json from the NLog-section
		/// </summary>
		public static ISetupBuilder LoadNLogConfigurationFromAppSettings(this ISetupBuilder setupBuilder, IConfiguration config)
		{
			const string NLOG_CONFIG_SECTION = "NLog";

			var nLogFromSection = config.GetSection("NLog")?.GetChildren();

			if (!string.IsNullOrEmpty(NLOG_CONFIG_SECTION) && nLogFromSection?.Any() == true)
			{
				return setupBuilder.LoadConfigurationFromSection(config);
			}

			setupBuilder.SetupExtensions(e => e.RegisterConfigSettings(config));

			return setupBuilder.LoadConfigurationFromFile();	// No effect, if config already loaded
		}

		/// <summary>
		/// Use for troubleshooting only.
		/// <para>The internal log file can be found in bin\Debug\net5.0\NLog.</para>
		/// </summary>
		public static string GetNLogInternalFullPath()
		{
			var today = DateTime.Today.ToString("yyyy-MM-dd");
			var binDirectory = Path.GetDirectoryName(typeof(Program).Assembly.Location);

			binDirectory ??= string.Empty;

			var nLogInternalFullPath = Path.Combine(binDirectory, "NLog", $"{today}-internal.log");

			return nLogInternalFullPath;
		}
	}
}
