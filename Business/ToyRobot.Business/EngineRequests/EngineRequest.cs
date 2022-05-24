using System.Drawing;
using ToyRobot.Infrastructure;
using ToyRobot.Infrastructure.Configurations;

namespace ToyRobot.Business.EngineRequests;

public class EngineRequest
{
	public TableDimensionConfig TableDimensionConfig { get; }

	public string Input { get; }

	public Point CurrentLocation { get; }

	public Direction Direction { get; }

	public EngineRequest(TableDimensionConfig tableDimensionConfig
		, string? input
		, Point currentLocation
		, Direction direction)
	{
		TableDimensionConfig = tableDimensionConfig;
		Input = input ?? string.Empty;
		CurrentLocation = currentLocation;
		Direction = direction;
	}
}
