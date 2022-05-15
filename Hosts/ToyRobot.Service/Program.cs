using NLog;
using NLog.Extensions.Logging;
using ToyRobot.Service.Extensions;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace ToyRobot.Service;

public class Program
{
	public static async Task Main(string[] args)
	{
		var host = CreateHostBuilder(args).Build();

		await host.RunAsync();
	}

	public static IHostBuilder CreateHostBuilder(string[] args) =>
		Host.CreateDefaultBuilder(args)
			.ConfigureLogging((hostBuilderContext, loggingBuilder) =>
			{
				LogManager.Setup()
					.LoadConfigurationFromAppSettings(hostBuilderContext.Configuration);
				loggingBuilder
					.ClearProviders()
					.SetMinimumLevel(LogLevel.Trace)
					.AddNLog()
					;
			})
			.ConfigureServices((hostBuilderContext, serviceCollection) =>
			{
				serviceCollection.PostConfigure<HostOptions>(option =>
				{
					option.ShutdownTimeout = TimeSpan.FromSeconds(60);
				});

				var configuration = hostBuilderContext.Configuration;

				serviceCollection
					.ConfigureSettingsOptions(configuration)
					.RegisterServices()
					.AddHostedService<Worker>();
			});
}
