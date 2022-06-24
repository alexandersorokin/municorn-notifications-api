namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Communication.AsyncLocal
{
    public interface IAsyncLocalServiceProvider<out TService>
        where TService : notnull
    {
        TService Value { get; }
    }
}
