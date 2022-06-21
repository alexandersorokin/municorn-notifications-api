using Microsoft.Extensions.DependencyInjection;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.BeforeFixtureConstructor
{
    public interface IModule
    {
        void ConfigureServices(IServiceCollection serviceCollection);
    }
}
