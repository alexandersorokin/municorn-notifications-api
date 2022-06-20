using Microsoft.Extensions.DependencyInjection;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.BeforeFixtureConstructor
{
    internal interface IModule
    {
        void ConfigureServices(IServiceCollection serviceCollection);
    }
}
