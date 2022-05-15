using Microsoft.Extensions.Logging;
using System.Drawing;
using ToyRobot.Business.Contracts;
using ToyRobot.Business.EngineResponses;
using ToyRobot.Business.Extensions;
using ToyRobot.Infrastructure;
using ToyRobot.Infrastructure.Configurations;

namespace ToyRobot.Business;

public class ToyRobotWorker : IToyRobotWorker
{
	private readonly ILogger<ToyRobotWorker> logger;
	private readonly List<IEngine> engines;
	private EngineResponse engineResponse;

	public ToyRobotWorker(ILogger<ToyRobotWorker> logger, IEnumerable<IEngine>? engines)
	{
		this.logger = logger;
		this.engines = engines?.ToList() ?? new List<IEngine>();

		engineResponse = new EngineResponse(null, null, new Point(-1, -1), Direction.NONE);
	}

	public void Execute(string input, TableDimensionConfig tableDimensionConfig)
	{
		input = input.Trim();

		var command = input.GetCommand();
		var engineName = $"{command}Engine";

		var engine = engines.FirstOrDefault(x => x.Name.IsEqualTo(engineName));

		if (engine == null)
		{
			logger.LogWarning($"Command not supported: '{command}'. Please try again.");
		}
		else
		{
			engineResponse = engine.Execute(tableDimensionConfig, engineResponse, input);

			if (!string.IsNullOrWhiteSpace(engineResponse?.Error))
			{
				logger.LogWarning(engineResponse.Error);
			}
			else
			{
				if (!string.IsNullOrWhiteSpace(engineResponse?.Message))
				{
					logger.LogTrace(engineResponse.Message);
				}
			}
		}
	}
}
