using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.ComponentModel.Design;
using ToyRobot.Business.Contracts;
using ToyRobot.Infrastructure.Configurations;
using ToyRobot.Service;
using Xunit;

namespace ToyRobot.Tests.Integration.BaseClasses;

public class IntegrationTestFixture
{
	public static IServiceProvider ServiceProvider { get; private set; } = new ServiceContainer();

	public static List<IEngine> Engines { get; private set; } = new();

	public static TableDimensionConfig TableDimensionConfig { get; private set; } = new();

	public static IOptions<TableDimensionConfig>? TableDimensionConfigOption { get; private set; }

	public IntegrationTestFixture()
	{
		ServiceProvider = Program.CreateHostBuilder(null!).Build()
			.Services;

		Engines = ServiceProvider.GetRequiredService<IEnumerable<IEngine>>().ToList();

		var tableDimensionConfig = ServiceProvider.GetRequiredService<IOptions<TableDimensionConfig>>();

		TableDimensionConfigOption = tableDimensionConfig;
		TableDimensionConfig = tableDimensionConfig.Value;
	}

	[CollectionDefinition(nameof(IntegrationTestFixture))]
	public class IntegrationTestFixtureCollectionDefinition : ICollectionFixture<IntegrationTestFixture>
	{
		// This class has no code, and is never created. Its purpose is simply
		// to be the place to apply [CollectionDefinition] and all the
		// ICollectionFixture<> interfaces.
	}
}
