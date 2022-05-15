using Microsoft.Extensions.Logging;
using NSubstitute;
using ToyRobot.Business;
using ToyRobot.Tests.Integration.BaseClasses;
using ToyRobot.Tests.Integration.Fakes;
using Xunit;

namespace ToyRobot.Tests.Integration.EngineTests;

public class ToyRobotWorkerTests : IntegrationTestBase
{
	private readonly FakeLogger<ToyRobotWorker> logger;

	public ToyRobotWorkerTests()
	{
		logger = Substitute.For<FakeLogger<ToyRobotWorker>>();
	}

	[Theory]
	[InlineData("LEFT"                                     , "Call PLACE command first then try again."         , LogLevel.Trace)]
	[InlineData("MOVE"                                     , "Call PLACE command first then try again."         , LogLevel.Trace)]
	[InlineData("PLACE -1,0,NORTH|REPORT"                  , "Robot will fall to destruction. Please try again.", LogLevel.Trace)]
	[InlineData("PLACE 0,0,NORTHT"                         , "Invalid Direction: 'NORTHT'. Please try again."   , LogLevel.Warning)]
	[InlineData("PLACE 2,2"                                , "Invalid argument: '2,2'. Please try again."       , LogLevel.Warning)]
	[InlineData("PLACE 2,2,NORTH|MOVE|REPORT"              , "Output: 2,3,NORTH"                                , LogLevel.Trace)]
	[InlineData("PLACE 0,0,NORTH|MOVE|REPORT"              , "Output: 0,1,NORTH"                                , LogLevel.Trace)]
	[InlineData("PLACE 0,0,NORTH|LEFT|REPORT"              , "Output: 0,0,WEST"                                 , LogLevel.Trace)]
	[InlineData("PLACE 1,2,EAST|MOVE|MOVE|LEFT|MOVE|REPORT", "Output: 3,3,NORTH"                                , LogLevel.Trace)]
	[InlineData("PLACE 2,y,NORTH"                          , "Invalid argument: 'y'. Please try again."         , LogLevel.Warning)]
	[InlineData("PLACE 4,4,NORTH|MOVE|REPORT"              , "Robot will fall to destruction. Please try again.", LogLevel.Trace)]
	[InlineData("PLACE x,2,NORTH"                          , "Invalid argument: 'x'. Please try again."         , LogLevel.Warning)]
	[InlineData("REPORT"                                   , "Call PLACE command first then try again."         , LogLevel.Trace)]
	[InlineData("RIGHT"                                    , "Call PLACE command first then try again."         , LogLevel.Trace)]
	[InlineData("asdf"                                     , "Command not supported: 'asdf'. Please try again." , LogLevel.Warning)]
	public void ShouldExecuteInputs(string input, string expectedLogMessage, LogLevel logLevel)
	{
		var toyRobotWorker = new ToyRobotWorker(logger, Engines);

		var inputs = input.Split('|');

		foreach (var command in inputs)
		{
			toyRobotWorker.Execute(command, TableDimensionConfig);
		}

		logger.Received(1).Log(logLevel, Arg.Is<string>(logMessage => logMessage == expectedLogMessage));
	}
}
