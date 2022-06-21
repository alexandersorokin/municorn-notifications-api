using Microsoft.Extensions.DependencyInjection;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor
{
    [DependencyInjectionContainer]
    public interface IConfigureServices
    {
        void ConfigureServices(IServiceCollection serviceCollection);
    }
}
