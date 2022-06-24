using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection;

namespace Municorn.Notifications.Api.Tests.ApiTests
{
    [FieldInjectionModule]
    internal interface IServiceFixture : IFixtureServiceProviderFramework
    {
        void IFixtureServiceProviderFramework.ConfigureServices(IServiceCollection serviceCollection) => serviceCollection.AddClientFactory();
    }
}
