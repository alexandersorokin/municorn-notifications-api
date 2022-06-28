namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureBuilder.Modules.Scoped
{
    internal interface IRegisterScoped<out TService>
    {
        TService Get();
    }
}
