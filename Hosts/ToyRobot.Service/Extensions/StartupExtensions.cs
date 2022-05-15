using NLog;
using NLog.Config;
using NLog.Extensions.Logging;
using ToyRobot.Infrastructure.Configurations;
using ToyRobot.Infrastructure.Contracts;

namespace ToyRobot.Service.Extensions;

public static class StartupExtensions
{
	public static IServiceCollection ConfigureSettingsOptions(this IServiceCollection services, IConfiguration configuration)
	{
		return services
			.Configure<TableDimensionConfig>(configuration.GetSection("TableDimension"));
	}

	/// <summary>
	/// Centralized location for registering services.
	/// </summary>
	public static IServiceCollection RegisterServices(this IServiceCollection services)
	{
		return services
			.Scan(scan => scan
				.FromApplicationDependencies()

				// IDiDiscoverableSingleton
				.AddClasses(classes => classes.AssignableTo(typeof(IDiDiscoverableSingleton)))
				.AsSelfWithInterfaces()
				.WithSingletonLifetime()

				// IDiDiscoverableScoped
				.AddClasses(classes => classes.AssignableTo(typeof(IDiDiscoverableScoped)))
				.AsSelfWithInterfaces()
				.WithScopedLifetime()

				// IDiDiscoverableTransient
				.AddClasses(classes => classes.AssignableTo(typeof(IDiDiscoverableTransient)))
				.AsSelfWithInterfaces()
				.WithTransientLifetime()
			);
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

	/// <summary>
	/// Loads NLog LoggingConfiguration from appsettings.json from the NLog-section
	/// </summary>
	public static ISetupBuilder LoadConfigurationFromAppSettings(this ISetupBuilder setupBuilder, IConfiguration config)
	{
		const string NLOG_CONFIG_SECTION = "NLog";

		var nlogFromSection = config.GetSection("NLog")?.GetChildren();

		if (!string.IsNullOrEmpty(NLOG_CONFIG_SECTION) && nlogFromSection?.Any() == true)
		{
			return setupBuilder.LoadConfigurationFromSection(config);
		}

		setupBuilder.SetupExtensions(e => e.RegisterConfigSettings(config));

		return setupBuilder.LoadConfigurationFromFile();    // No effect, if config already loaded
	}
}
