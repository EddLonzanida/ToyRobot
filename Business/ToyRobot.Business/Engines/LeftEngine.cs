using ToyRobot.Business.BaseClasses;
using ToyRobot.Business.Contracts;
using ToyRobot.Business.EngineRequests;
using ToyRobot.Business.EngineResponses;
using ToyRobot.Business.Extensions;
using ToyRobot.Infrastructure;

namespace ToyRobot.Business.Engines;

/// <summary>
/// <inheritdoc cref="IEngine"/>
/// </summary>
public class LeftEngine : EngineBase<LeftEngine>
{
	public override EngineResponse Execute(EngineRequest request)
	{
		if (request.Direction == Direction.NONE)
		{
			return request.GetResponseWithMessage(PLACE_COMMAND_NOT_CALLED);
		}

		var x = request.CurrentLocation.X;
		var y = request.CurrentLocation.Y;

		Direction direction;

		switch (request.Direction)
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
				return request.ToResponse();
		}

		var tmpEngineResponse = request.ToResponse(direction);

		return GetResponse(x, y, tmpEngineResponse);
	}
}
