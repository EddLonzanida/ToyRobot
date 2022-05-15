using Microsoft.Extensions.Logging;

namespace ToyRobot.Tests.Integration.Fakes;

public abstract class FakeLogger<T> : ILogger<T>
{
	void ILogger.Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
		=> Log(logLevel, formatter(state, exception), exception);

	public abstract void Log(LogLevel logLevel, string message, Exception? exception = null);

	public virtual bool IsEnabled(LogLevel logLevel) => true;

	public abstract IDisposable BeginScope<TState>(TState state);
}
