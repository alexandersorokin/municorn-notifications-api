namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.BeforeFixtureConstructor.Scoped
{
    internal interface IScoped<out TService>
    {
        TService Get();
    }
}
