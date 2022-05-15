using ToyRobot.Business.EngineResponses;
using ToyRobot.Infrastructure.Configurations;
using ToyRobot.Infrastructure.Contracts;

namespace ToyRobot.Business.Contracts;

/// <summary>
/// <inheritdoc cref="IDiDiscoverableTransient"/>
/// </summary>
public interface IEngine : IDiDiscoverableTransient
{
	string Name { get; }
	EngineResponse Execute(TableDimensionConfig tableDimensionConfig, EngineResponse engineResponse, string input);
}
