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
public class RightEngine : EngineBase<RightEngine>
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
				direction = Direction.EAST;
				break;
			case Direction.SOUTH:
				direction = Direction.WEST;
				break;
			case Direction.WEST:
				direction = Direction.NORTH;
				break;
			case Direction.EAST:
				direction = Direction.SOUTH;
				break;
			default:
				return engineResponse;
		}

		var tmpEngineResponse = direction.GetEngineResponse(x, y);

		return GetResponse(tableDimensionConfig, x, y, tmpEngineResponse);
	}
}
