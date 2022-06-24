using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Fields;

namespace Municorn.Notifications.Api.Tests.ApiTests
{
    [FieldDependenciesModule]
    internal interface IServiceFixture : ITestFixture
    {
        void ITestFixture.ConfigureServices(IServiceCollection serviceCollection) => serviceCollection.AddClientFactory();
    }
}
