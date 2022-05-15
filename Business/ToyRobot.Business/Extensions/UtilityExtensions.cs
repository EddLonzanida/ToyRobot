using System.Drawing;
using System.Text.RegularExpressions;
using ToyRobot.Business.EngineResponses;
using ToyRobot.Infrastructure;

namespace ToyRobot.Business.Extensions;

public static class UtilityExtensions
{
	public static string GetCommand(this string input)
	{
		const string REGEX_PATTERN = @"^\w+";

		var regex = new Regex(REGEX_PATTERN);
		var match = regex.Match(input);

		if (match.Success)
		{
			return match.Value;
		}

		return string.Empty;
	}

	public static EngineResponse GetResponseWithError(this string error)
	{
		return new EngineResponse(null, error, new Point(-1, -1), Direction.NONE);
	}

	public static EngineResponse GetResponseWithMessage(this string message)
	{
		return new EngineResponse(message, null, new Point(-1, -1), Direction.NONE);
	}

	public static EngineResponse GetEngineResponse(this Direction direction, int x, int y)
	{
		return new EngineResponse(null, null, new Point(x, y), direction);
	}

	public static EngineResponse GetResponseWithMessage(this EngineResponse engineResponse, string message)
	{
		return new EngineResponse(message, null, engineResponse.CurrentLocation, engineResponse.Direction);
	}
	/// <summary>
	/// Case insensitive comparison. Trims both words before comparing.
	/// </summary>
	public static bool IsEqualTo(this string source, string value)
	{
		return string.Equals(source.Trim(), value.Trim(), StringComparison.CurrentCultureIgnoreCase);
	}
}
