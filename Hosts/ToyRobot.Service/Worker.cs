using Microsoft.Extensions.Options;
using ToyRobot.Business.Contracts;
using ToyRobot.Infrastructure.Configurations;

namespace ToyRobot.Service;

public class Worker : BackgroundService
{
	private readonly ILogger<Worker> logger;
	private readonly TableDimensionConfig tableDimensionConfig;
	private readonly IToyRobotWorker toyRobotWorker;

	public Worker(ILogger<Worker> logger
		, IOptions<TableDimensionConfig> tableDimensionConfig
		, IToyRobotWorker toyRobotWorker)
	{
		this.logger = logger;
		this.toyRobotWorker = toyRobotWorker;
		this.tableDimensionConfig = tableDimensionConfig.Value;
	}

	protected override async Task ExecuteAsync(CancellationToken cancellationToken)
	{
		try
		{
			logger.LogTrace($"Welcome to TOY ROBOT!{Environment.NewLine}");
			logger.LogInformation($"Table top dimension-> Height:{tableDimensionConfig.Height} Width:{tableDimensionConfig.Width}");
			logger.LogDebug("Type your command then press ENTER to accept..");

			while (!cancellationToken.IsCancellationRequested)
			{
				var input = Console.ReadLine() ?? string.Empty;

				await Task.Run(async () =>
				{
					await Task.Delay(10, cancellationToken);

					toyRobotWorker.Execute(input, tableDimensionConfig);

				}, cancellationToken);
			}
		}
		catch (TaskCanceledException)
		{
			//Ignore
			//A task was canceled.
		}
		catch (Exception e)
		{
			logger.LogError(e, e.Message);
		}
		finally
		{
			Console.WriteLine();
			Console.WriteLine();
			logger.LogInformation("Press Enter to exit..");
			Console.WriteLine();
			Console.WriteLine();
			Console.ReadLine();
		}
	}
}
