using ToyRobot.Business.Contracts;

namespace ToyRobot.Service;

public class Worker : BackgroundService
{
	private readonly ILogger<Worker> logger;
	private readonly IToyRobotWorker toyRobotWorker;

	public Worker(ILogger<Worker> logger, IToyRobotWorker toyRobotWorker)
	{
		this.logger = logger;
		this.toyRobotWorker = toyRobotWorker;
	}

	protected override async Task ExecuteAsync(CancellationToken cancellationToken)
	{
		try
		{
			logger.LogTrace($"Welcome to TOY ROBOT!{Environment.NewLine}");
			logger.LogDebug("Type your command then press ENTER to accept..");

			while (!cancellationToken.IsCancellationRequested)
			{
				var input = Console.ReadLine() ?? string.Empty;

				await Task.Run(async () =>
				{
					await Task.Delay(10, cancellationToken);

					toyRobotWorker.Execute(input);

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
