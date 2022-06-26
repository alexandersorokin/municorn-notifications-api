using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureActions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureActions.Modules.Attributes.ViaInterface
{
    [TestMethodInjectionModule]
    internal interface IFixtureServiceProviderWithMethodInjection : IFixtureWithServiceProviderFramework
    {
    }
}
