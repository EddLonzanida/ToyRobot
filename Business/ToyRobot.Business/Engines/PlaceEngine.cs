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
public class PlaceEngine : EngineBase<PlaceEngine>
{
	public override EngineResponse Execute(EngineRequest request)
	{
		var input = request.Input ?? string.Empty;
		var aSegments = input.Split(' ');

		if (aSegments.Length < 2)
		{
			return request.GetResponseWithError(INVALID_COMMAND);
		}
		var command = input.GetCommand();
		var arguments = input.Replace(command, string.Empty).Trim();
		var aArguments = arguments.Split(',').ToList();

		if (aArguments.Count != 3)
		{
			var msg = string.Format(INVALID_ARGUMENT, arguments);

			return request.GetResponseWithError(msg);
		}

		aArguments = aArguments.ConvertAll(a => a.Trim());

		var xAsString = aArguments[0];
		var yAsString = aArguments[1];
		var directionAsString = aArguments[2];

		if (!int.TryParse(xAsString, out var x))
		{
			var msg = string.Format(INVALID_ARGUMENT, xAsString);

			return request.GetResponseWithError(msg);
		}

		if (!int.TryParse(yAsString, out var y))
		{
			var msg = string.Format(INVALID_ARGUMENT, yAsString);

			return request.GetResponseWithError(msg);
		}

		var direction = GetDirection(directionAsString);

		if (direction == Direction.NONE)
		{
			var msg = string.Format(INVALID_DIRECTION, directionAsString);

			return request.GetResponseWithError(msg);
		}

		var tmpEngineResponse = request.ToResponse(x, y, direction);

		return GetResponse(x, y, tmpEngineResponse);
	}

	private static Direction GetDirection(string directionAsString)
	{
		try
		{
			var direction = (Direction)Enum.Parse(typeof(Direction), directionAsString, true);

			return direction;
		}
		catch (Exception)
		{
			return Direction.NONE;
		}
	}
}
