using Microsoft.Extensions.DependencyInjection;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor
{
    [DependencyInjectionContainer]
    public interface ITestFixture
    {
        void ConfigureServices(IServiceCollection serviceCollection);
    }
}
