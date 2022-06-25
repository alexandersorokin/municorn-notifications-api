using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureActions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureActions
{
    internal interface IWithFields : IFixtureWithServiceProviderFramework
    {
        void IFixtureWithServiceProviderFramework.ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddFieldInjection(this);
            this.SetUpServices(serviceCollection);
        }

        void SetUpServices(IServiceCollection serviceCollection);
    }
}
