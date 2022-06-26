using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureActions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FixtureOneTimeActions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.TestCommunication;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureActions.Integration
{
    [TestCommunicationModule]
    [TestMethodInjectionModule]
    internal interface IWithDefaultModules : IFixtureWithServiceProviderFramework
    {
        void IFixtureWithServiceProviderFramework.ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddFieldInjection(this)
                .AddFixtureOneTimeActions();
            this.SetUpServices(serviceCollection);
        }

        void SetUpServices(IServiceCollection serviceCollection);
    }
}
