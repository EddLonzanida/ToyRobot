namespace ToyRobot.Infrastructure.Contracts;

/// <summary>
/// <para>DO NOT inject scoped or transient lifetime in singletons.</para>
/// <para><see href="https://docs.microsoft.com/en-us/dotnet/core/extensions/dependency-injection-guidelines"/></para>
/// <para>Singleton.</para>
/// </summary>
public interface IDiDiscoverableSingleton
{
}
