using Microsoft.Extensions.DependencyInjection;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor
{
    [ServiceProviderFramework]
    public interface IFixtureServiceProviderFramework
    {
        void ConfigureServices(IServiceCollection serviceCollection);
    }
}
