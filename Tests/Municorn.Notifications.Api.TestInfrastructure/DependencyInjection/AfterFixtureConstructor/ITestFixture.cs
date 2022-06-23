using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor.Fields;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor
{
    [DependencyInjectionContainer]
    [FieldDependencyModule]
    public interface ITestFixture
    {
        void ConfigureServices(IServiceCollection serviceCollection);
    }
}
