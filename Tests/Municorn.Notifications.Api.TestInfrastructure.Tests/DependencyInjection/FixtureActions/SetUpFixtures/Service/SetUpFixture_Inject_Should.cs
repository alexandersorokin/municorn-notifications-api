using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureActions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureActions.SetUpFixtures.Service
{
    [TestFixture]
    internal class SetUpFixture_Inject_Should : IFixtureWithServiceProviderFramework
    {
        public void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection
            .AddTestMethodInjection()
            .AddScoped<MockService>();

        [Test]
        [Repeat(2)]
        public void Inject_Service([InjectDependency] MockService service) => service.Should().NotBeNull();

        [Test]
        [Repeat(2)]
        public void Inject_Fixture([InjectDependency] SetUpFixture fixtureService) =>
            fixtureService.Service.Should().NotBeNull();

        [TestCaseSource(nameof(InjectParentServiceCase))]
        [Repeat(2)]
        public void Inject_SetUpFixture_Service(SetUpFixture fixtureService, MockService setUpFixtureService) =>
            fixtureService.Service.Should().Be(setUpFixtureService);

        private static readonly TestCaseData[] InjectParentServiceCase =
        {
            new(new InjectedService<SetUpFixture>(), new InjectServiceFromSetUpFixture()),
        };

        private class InjectServiceFromSetUpFixture : IInjectedService
        {
            public Type? GetServiceType(object? methodCallTargetFixture, object containerFixture) =>
                methodCallTargetFixture == containerFixture
                    ? null
                    : typeof(MockService);
        }
    }
}
