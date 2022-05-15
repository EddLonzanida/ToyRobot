using ToyRobot.Business.BaseClasses;
using ToyRobot.Business.Contracts;
using ToyRobot.Business.EngineResponses;
using ToyRobot.Business.Extensions;
using ToyRobot.Infrastructure;
using ToyRobot.Infrastructure.Configurations;

namespace ToyRobot.Business.Engines;

/// <summary>
/// <inheritdoc cref="IEngine"/>
/// </summary>
public class ReportEngine : EngineBase<ReportEngine>
{
	public override EngineResponse Execute(TableDimensionConfig tableDimensionConfig, EngineResponse engineResponse, string input)
	{
		if (engineResponse.Direction == Direction.NONE)
		{
			return PLACE_COMMAND_NOT_CALLED.GetResponseWithMessage();
		}

		var x = engineResponse.CurrentLocation.X;
		var y = engineResponse.CurrentLocation.Y;
		var compass = engineResponse.Direction;
		var message = $"Output: {x},{y},{compass}";

		return new EngineResponse(message, null, engineResponse.CurrentLocation, engineResponse.Direction);
	}
}
