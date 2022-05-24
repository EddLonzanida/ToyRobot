using System.Drawing;
using System.Text.RegularExpressions;
using ToyRobot.Business.EngineRequests;
using ToyRobot.Business.EngineResponses;
using ToyRobot.Infrastructure;

namespace ToyRobot.Business.Extensions;

public static class UtilityExtensions
{
	public static string GetCommand(this string input)
	{
		const string REGEX_PATTERN = @"^\w+"; // get the first word

		var regex = new Regex(REGEX_PATTERN);
		var match = regex.Match(input);

		return match.Success ? match.Value : string.Empty;
	}

	public static EngineResponse GetResponseWithMessage(this EngineResponse engineResponse, string message)
	{
		return new EngineResponse(message, null, engineResponse.CurrentLocation, engineResponse.Direction, engineResponse.TableDimensionConfig);
	}

	public static EngineResponse GetResponseWithMessage(this EngineRequest engineResponse, string message)
	{
		return new EngineResponse(message, null, engineResponse.CurrentLocation, engineResponse.Direction, engineResponse.TableDimensionConfig);
	}

	public static EngineResponse GetResponseWithError(this EngineRequest engineResponse, string message)
	{
		return new EngineResponse(null, message, engineResponse.CurrentLocation, engineResponse.Direction, engineResponse.TableDimensionConfig);
	}

	public static EngineResponse ToResponse(this EngineRequest request)
	{
		return new EngineResponse(null, null, request.CurrentLocation, request.Direction, request.TableDimensionConfig);
	}

	public static EngineResponse ToResponse(this EngineRequest request, Direction direction)
	{
		return new EngineResponse(null, null, request.CurrentLocation, direction, request.TableDimensionConfig);
	}

	public static EngineResponse ToResponse(this EngineRequest request, int x, int y, Direction direction)
	{
		var newLocation = new Point(x, y);

		return new EngineResponse(null, null, newLocation, direction, request.TableDimensionConfig);
	}

	public static EngineRequest ToRequest(this EngineResponse response, string input)
	{
		return new EngineRequest(response.TableDimensionConfig, input, response.CurrentLocation, response.Direction);
	}

	public static EngineRequest ToRequest(this EngineResponse response)
	{
		return new EngineRequest(response.TableDimensionConfig, string.Empty, response.CurrentLocation, response.Direction);
	}

	/// <summary>
	/// Case insensitive comparison. Trims both words before comparing.
	/// </summary>
	public static bool IsEqualTo(this string source, string value)
	{
		return string.Equals(source.Trim(), value.Trim(), StringComparison.CurrentCultureIgnoreCase);
	}
}
