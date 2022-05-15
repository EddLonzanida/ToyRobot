using System.Drawing;
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
public class PlaceEngine : EngineBase<PlaceEngine>
{
	public override EngineResponse Execute(TableDimensionConfig tableDimensionConfig, EngineResponse engineResponse, string input)
	{
		var aSegments = input.Split(' ');

		if (aSegments.Length < 2)
		{
			return INVALID_COMMAND.GetResponseWithError();
		}
		var command = input.GetCommand();
		var arguments = input.Replace(command, string.Empty).Trim();
		var aArguments = arguments.Split(',').ToList();

		if (aArguments.Count != 3)
		{
			var msg = string.Format(INVALID_ARGUMENT, arguments);

			return msg.GetResponseWithError();
		}

		aArguments = aArguments.ConvertAll(a => a.Trim());

		var xAsString = aArguments[0];
		var yAsString = aArguments[1];
		var compassAsString = aArguments[2];

		if (!int.TryParse(xAsString, out var x))
		{
			var msg = string.Format(INVALID_ARGUMENT, xAsString);

			return msg.GetResponseWithError();
		}

		if (!int.TryParse(yAsString, out var y))
		{
			var msg = string.Format(INVALID_ARGUMENT, yAsString);

			return msg.GetResponseWithError();
		}

		var compass = GetCompass(compassAsString);

		if (compass == Direction.NONE)
		{
			var msg = string.Format(INVALID_DIRECTION, compassAsString);

			return msg.GetResponseWithError();
		}

		var currentLocation = new Point(x, y);
		var tmpEngineResponse = new EngineResponse(null, null, currentLocation, compass);

		return GetResponse(tableDimensionConfig, x, y, tmpEngineResponse);
	}

	private static Direction GetCompass(string compassAsString)
	{
		try
		{
			var compass = (Direction)Enum.Parse(typeof(Direction), compassAsString, true);

			return compass;
		}
		catch (Exception)
		{
			return Direction.NONE;
		}
	}
}
