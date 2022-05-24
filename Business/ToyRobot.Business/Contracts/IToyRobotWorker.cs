using ToyRobot.Infrastructure.Contracts;

namespace ToyRobot.Business.Contracts;

public interface IToyRobotWorker : IDiDiscoverableTransient
{
	void Execute(string input);
}
