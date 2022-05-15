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
public class MoveEngine : EngineBase<MoveEngine>
{
	public override EngineResponse Execute(TableDimensionConfig tableDimensionConfig, EngineResponse engineResponse, string input)
	{
		if (engineResponse.Direction == Direction.NONE)
		{
			return PLACE_COMMAND_NOT_CALLED.GetResponseWithMessage();
		}

		var x = engineResponse.CurrentLocation.X;
		var y = engineResponse.CurrentLocation.Y;

		switch (engineResponse.Direction)
		{
			case Direction.NORTH:
				y += 1;
				break;
			case Direction.SOUTH:
				y -= 1;
				break;
			case Direction.WEST:
				x -= 1;
				break;
			case Direction.EAST:
				x += 1;
				break;
			default:
				return engineResponse;
		}

		return GetResponse(tableDimensionConfig, x, y, engineResponse);
	}
}
