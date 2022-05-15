using ToyRobot.Business.Contracts;
using ToyRobot.Business.Extensions;
using ToyRobot.Infrastructure.Configurations;
using Xunit;

namespace ToyRobot.Tests.Integration.BaseClasses;

/// <summary>
/// <inheritdoc cref="IntegrationTestFixture"/>
/// </summary>
[Collection(nameof(IntegrationTestFixture))]
public abstract class IntegrationTestBase
{
	protected readonly IServiceProvider ServiceProvider;
	protected readonly List<IEngine> Engines;
	protected readonly IEngine ReportEngine;
	protected readonly TableDimensionConfig TableDimensionConfig;

	protected IntegrationTestBase()
	{
		ServiceProvider = IntegrationTestFixture.ServiceProvider;
		Engines = IntegrationTestFixture.Engines;
		TableDimensionConfig = IntegrationTestFixture.TableDimensionConfig;

		ReportEngine = Engines.First(x => x.Name.IsEqualTo(nameof(ReportEngine)));
	}
}
