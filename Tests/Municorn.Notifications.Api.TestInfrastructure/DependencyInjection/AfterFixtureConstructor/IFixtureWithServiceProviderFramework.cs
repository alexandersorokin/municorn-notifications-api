using Microsoft.Extensions.DependencyInjection;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor
{
    [ServiceProviderFramework]
    public interface IFixtureWithServiceProviderFramework
    {
        void ConfigureServices(IServiceCollection serviceCollection);
    }
}
