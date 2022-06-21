using Microsoft.Extensions.DependencyInjection;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.BeforeFixtureConstructor
{
    public interface IModule
    {
        void ConfigureServices(IServiceCollection serviceCollection);
    }
}
