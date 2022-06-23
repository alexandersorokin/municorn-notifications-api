namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.BeforeFixtureConstructor.Modules.Scoped
{
    internal interface IScoped<out TService>
    {
        TService Get();
    }
}
