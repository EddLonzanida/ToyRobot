using Shouldly;
using System.Drawing;
using ToyRobot.Business.EngineResponses;
using ToyRobot.Business.Engines;
using ToyRobot.Business.Extensions;
using ToyRobot.Infrastructure;
using ToyRobot.Tests.Integration.BaseClasses;
using Xunit;

namespace ToyRobot.Tests.Integration.EngineTests;

public class EngineTests : IntegrationTestBase
{
	[Theory]
	[InlineData(Direction.EAST, 4, 4, "Output: 4,4,EAST")]
	[InlineData(Direction.EAST, 4, 0, "Output: 4,0,EAST")]
	[InlineData(Direction.NORTH, 0, 4, "Output: 0,4,NORTH")]
	[InlineData(Direction.NORTH, 4, 4, "Output: 4,4,NORTH")]
	[InlineData(Direction.SOUTH, 4, 0, "Output: 4,0,SOUTH")]
	[InlineData(Direction.SOUTH, 0, 0, "Output: 0,0,SOUTH")]
	[InlineData(Direction.WEST, 0, 4, "Output: 0,4,WEST")]
	[InlineData(Direction.WEST, 0, 0, "Output: 0,0,WEST")]
	public void Engines_ShouldIgnoreOutOfBounds(Direction initialDirection, int initialLocationX, int initialLocationY, string expectedReport)
	{
		//initial placement in the four corners of 5x5 table
		var initialPlacement = new EngineResponse(null, null, new Point(initialLocationX, initialLocationY), initialDirection, TableDimensionConfig);
		var moveEngine = Engines.FirstOrDefault(x => x.Name.IsEqualTo(nameof(MoveEngine)));

		moveEngine.ShouldNotBeNull();

		// MOVE
		var request = initialPlacement.ToRequest();
		var engineResponse = moveEngine.Execute(request);

		//REPORT
		request = engineResponse.ToRequest();
		var reportEngineResponse = ReportEngine.Execute(request);

		reportEngineResponse.Message.ShouldBe(expectedReport);
		engineResponse.Error.ShouldBeNullOrWhiteSpace();
	}

	[Theory]
	[InlineData(Direction.EAST, 2, 2, "Output: 3,2,EAST")]
	[InlineData(Direction.NORTH, 2, 2, "Output: 2,3,NORTH")]
	[InlineData(Direction.SOUTH, 2, 2, "Output: 2,1,SOUTH")]
	[InlineData(Direction.WEST, 2, 2, "Output: 1,2,WEST")]
	public void Engines_ShouldNavigate(Direction initialDirection, int initialLocationX, int initialLocationY, string expectedReport)
	{
		var initialPlacement = new EngineResponse(null, null, new Point(initialLocationX, initialLocationY), initialDirection, TableDimensionConfig);
		var moveEngine = Engines.FirstOrDefault(x => x.Name.IsEqualTo(nameof(MoveEngine)));

		moveEngine.ShouldNotBeNull();

		// MOVE
		var request = initialPlacement.ToRequest();
		var engineResponse = moveEngine.Execute(request);

		//REPORT
		request = engineResponse.ToRequest();
		var reportEngineResponse = ReportEngine.Execute(request);

		reportEngineResponse.Message.ShouldBe(expectedReport);
		engineResponse.Error.ShouldBeNullOrWhiteSpace();
	}
}
