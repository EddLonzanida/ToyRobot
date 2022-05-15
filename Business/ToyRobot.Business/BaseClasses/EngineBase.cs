using System.Drawing;
using ToyRobot.Business.Contracts;
using ToyRobot.Business.EngineResponses;
using ToyRobot.Business.Extensions;
using ToyRobot.Infrastructure.Configurations;

namespace ToyRobot.Business.BaseClasses;

public abstract class EngineBase<T> : IEngine
	where T : IEngine
{
	protected const string INVALID_COMMAND = "Invalid command. Please try again.";
	protected const string INVALID_ARGUMENT = "Invalid argument: '{0}'. Please try again.";
	protected const string INVALID_DIRECTION = "Invalid Direction: '{0}'. Please try again.";
	protected const string PLACE_COMMAND_NOT_CALLED = "Call PLACE command first then try again.";
	protected const string ROBOT_WILL_FALL = "Robot will fall to destruction. Please try again.";

	public string Name => typeof(T).Name;

	public abstract EngineResponse Execute(TableDimensionConfig tableDimensionConfig, EngineResponse engineResponse, string input);

	/// <summary>
	/// Check the new location if valid.
	/// </summary>
	protected EngineResponse GetResponse(TableDimensionConfig tableDimensionConfig, int newLocationX, int newLocationY, EngineResponse engineResponse)
	{
		if (newLocationX < 0 || newLocationX > tableDimensionConfig.Width - 1)
		{
			return engineResponse.GetResponseWithMessage(ROBOT_WILL_FALL);
		}

		if (newLocationY < 0 || newLocationY > tableDimensionConfig.Height - 1)
		{
			return engineResponse.GetResponseWithMessage(ROBOT_WILL_FALL);
		}

		var currentLocation = new Point(newLocationX, newLocationY);

		return new EngineResponse(null, null, currentLocation, engineResponse.Direction);
	}
}
