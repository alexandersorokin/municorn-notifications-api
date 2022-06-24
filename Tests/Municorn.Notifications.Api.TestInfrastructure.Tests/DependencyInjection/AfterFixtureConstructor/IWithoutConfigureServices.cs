using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.AfterFixtureConstructor
{
    internal interface IWithoutConfigureServices : IFixtureServiceProviderFramework
    {
        void IFixtureServiceProviderFramework.ConfigureServices(IServiceCollection serviceCollection)
        {
            // Nothing by default
        }
    }
}
