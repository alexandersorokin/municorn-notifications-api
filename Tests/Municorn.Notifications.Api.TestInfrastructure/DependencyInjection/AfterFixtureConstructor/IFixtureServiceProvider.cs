using Microsoft.Extensions.DependencyInjection;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor
{
    [DependencyInjectionContainer]
    public interface IFixtureServiceProvider
    {
        void ConfigureServices(IServiceCollection serviceCollection);
    }
}
