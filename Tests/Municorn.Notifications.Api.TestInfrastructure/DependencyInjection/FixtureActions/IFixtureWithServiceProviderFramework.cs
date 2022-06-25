using Microsoft.Extensions.DependencyInjection;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureActions
{
    [ServiceProviderFramework]
    public interface IFixtureWithServiceProviderFramework
    {
        void ConfigureServices(IServiceCollection serviceCollection);
    }
}
