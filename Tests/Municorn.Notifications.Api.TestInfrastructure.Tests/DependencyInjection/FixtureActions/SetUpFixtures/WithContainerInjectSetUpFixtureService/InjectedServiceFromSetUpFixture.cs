using System;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureActions.SetUpFixtures.WithContainerInjectSetUpFixtureService
{
    internal class InjectedServiceFromSetUpFixture : IInjectedService
    {
        public Type? GetServiceType(object? methodCallTargetFixture, object containerFixture) =>
            methodCallTargetFixture == containerFixture
                ? null
                : typeof(MockService);
    }
}