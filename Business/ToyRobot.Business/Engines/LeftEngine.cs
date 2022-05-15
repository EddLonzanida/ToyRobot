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
public class LeftEngine : EngineBase<LeftEngine>
{
	public override EngineResponse Execute(TableDimensionConfig tableDimensionConfig, EngineResponse engineResponse, string input)
	{
		if (engineResponse.Direction == Direction.NONE)
		{
			return PLACE_COMMAND_NOT_CALLED.GetResponseWithMessage();
		}

		var x = engineResponse.CurrentLocation.X;
		var y = engineResponse.CurrentLocation.Y;

		Direction direction;

		switch (engineResponse.Direction)
		{
			case Direction.NORTH:
				direction = Direction.WEST;
				break;
			case Direction.SOUTH:
				direction = Direction.EAST;
				break;
			case Direction.WEST:
				direction = Direction.SOUTH;
				break;
			case Direction.EAST:
				direction = Direction.NORTH;
				break;
			default:
				return engineResponse;
		}

		var tmpEngineResponse = direction.GetEngineResponse(x, y);

		return GetResponse(tableDimensionConfig, x, y, tmpEngineResponse);
	}
}
