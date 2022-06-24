using Microsoft.Extensions.DependencyInjection;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor
{
    [ServiceProvider]
    public interface IFixtureServiceProvider
    {
        void ConfigureServices(IServiceCollection serviceCollection);
    }
}
