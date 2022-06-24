namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules
{
    public interface IAsyncLocalServiceProvider<out TService>
        where TService : notnull
    {
        TService Value { get; }
    }
}
