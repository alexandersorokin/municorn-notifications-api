using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureActions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureActions.ImplicitInterface
{
    internal interface IWithNoServices : IFixtureWithServiceProviderFramework
    {
        void IFixtureWithServiceProviderFramework.ConfigureServices(IServiceCollection serviceCollection) =>
            serviceCollection.AddFieldInjection(this);
    }
}
