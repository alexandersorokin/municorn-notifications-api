using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor.Fields;

namespace Municorn.Notifications.Api.Tests.ApiTests
{
    [FieldDependencyModule]
    internal interface IServiceFixture : ITestFixture
    {
        void ITestFixture.ConfigureServices(IServiceCollection serviceCollection) => serviceCollection.RegisterClientFactory();
    }
}
