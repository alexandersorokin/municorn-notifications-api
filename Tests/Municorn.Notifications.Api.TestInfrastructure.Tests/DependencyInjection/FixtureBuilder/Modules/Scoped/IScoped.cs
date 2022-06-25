namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureBuilder.Modules.Scoped
{
    internal interface IScoped<out TService>
    {
        TService Get();
    }
}
