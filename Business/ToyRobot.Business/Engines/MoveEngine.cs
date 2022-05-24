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
public class MoveEngine : EngineBase<MoveEngine>
{
	public override EngineResponse Execute(EngineRequest request)
	{
		if (request.Direction == Direction.NONE)
		{
			return request.GetResponseWithMessage(PLACE_COMMAND_NOT_CALLED);
		}

		var x = request.CurrentLocation.X;
		var y = request.CurrentLocation.Y;

		switch (request.Direction)
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
				return request.ToResponse();
		}

		var tmpEngineResponse = request.ToResponse();

		return GetResponse(x, y, tmpEngineResponse);
	}
}
