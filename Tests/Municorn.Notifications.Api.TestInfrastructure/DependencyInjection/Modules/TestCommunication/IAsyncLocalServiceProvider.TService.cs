namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.TestCommunication
{
    public interface IAsyncLocalServiceProvider<out TService>
        where TService : notnull
    {
        TService Value { get; }
    }
}
