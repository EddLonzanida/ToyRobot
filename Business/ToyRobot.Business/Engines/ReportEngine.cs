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
public class ReportEngine : EngineBase<ReportEngine>
{
	public override EngineResponse Execute(EngineRequest request)
	{
		if (request.Direction == Direction.NONE)
		{
			return request.GetResponseWithMessage(PLACE_COMMAND_NOT_CALLED);
		}

		var x = request.CurrentLocation.X;
		var y = request.CurrentLocation.Y;
		var direction = request.Direction;
		var message = $"Output: {x},{y},{direction}";

		return new EngineResponse(message, null, request.CurrentLocation, request.Direction, request.TableDimensionConfig);
	}
}
