using System.Drawing;
using ToyRobot.Infrastructure;

namespace ToyRobot.Business.EngineResponses;

public class EngineResponse
{
	/// <summary>
	/// Use this if you want to use logger.LogDebug.
	/// </summary>
	public string? Message { get; }

	/// <summary>
	/// Use this if you want to use logger.LogWarning.
	/// </summary>
	public string? Error { get; }

	public Point CurrentLocation { get; }

	public Direction Direction { get; }

	public EngineResponse(string? message, string? error, Point currentLocation, Direction direction)
	{
		Message = message;
		Error = error;
		CurrentLocation = currentLocation;
		Direction = direction;
	}
}
