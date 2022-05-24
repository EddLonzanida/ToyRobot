using ToyRobot.Business.EngineRequests;
using ToyRobot.Business.EngineResponses;
using ToyRobot.Infrastructure.Contracts;

namespace ToyRobot.Business.Contracts;

/// <summary>
/// <inheritdoc cref="IDiDiscoverableTransient"/>
/// </summary>
public interface IEngine : IDiDiscoverableTransient
{
	string Name { get; }
	EngineResponse Execute(EngineRequest request);
}
