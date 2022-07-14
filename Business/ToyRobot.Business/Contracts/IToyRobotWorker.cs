using ToyRobot.Infrastructure.Contracts;

namespace ToyRobot.Business.Contracts;

public interface IToyRobotWorker : IDiDiscoverableTransient
{
	/// <summary>
	/// Dynamically pickup the correct Engine suited to execute the command.
	/// </summary>
	void Execute(string input);
}
