using System.Drawing;
using ToyRobot.Infrastructure;
using ToyRobot.Infrastructure.Configurations;

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

	public TableDimensionConfig TableDimensionConfig { get; }

	public EngineResponse(string? message
		, string? error
		, Point currentLocation
		, Direction direction
		, TableDimensionConfig? tableDimensionConfig)
	{
		Message = message;
		Error = error;
		CurrentLocation = new Point(currentLocation.X, currentLocation.Y);
		Direction = direction;

		if (tableDimensionConfig == null)
		{
			TableDimensionConfig = new TableDimensionConfig();
		}
		else
		{
			TableDimensionConfig = new TableDimensionConfig
			{
				Height = tableDimensionConfig.Height,
				Width = tableDimensionConfig.Width
			};
		}
	}
}
