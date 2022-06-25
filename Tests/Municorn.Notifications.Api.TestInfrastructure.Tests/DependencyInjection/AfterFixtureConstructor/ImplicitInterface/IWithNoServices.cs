using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureActions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.AfterFixtureConstructor.ImplicitInterface
{
    internal interface IWithNoServices : IFixtureWithServiceProviderFramework
    {
        void IFixtureWithServiceProviderFramework.ConfigureServices(IServiceCollection serviceCollection)
        {
            // Nothing by default
        }
    }
}
