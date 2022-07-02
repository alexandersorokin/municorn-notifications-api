namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureBuilder.Modules.ForFixtureBuilderOnly.Scoped
{
    internal interface IRegisterScoped<out TService>
    {
        TService Get();
    }
}
