using System;
using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureBuilder;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureBuilder.SetUpFixtures.AfterFixtureConstructorCountChilds
{
    [TestFixtureInjectable]
    [TestMethodInjectionModule]
    [MockServiceModule]
    internal class SetUpFixture_Service_Should
    {
        [Test]
        [Repeat(2)]
        public void Inject_Case([InjectDependency] MockService service) => service.Should().NotBeNull();

        [Test]
        [Repeat(2)]
        public void SetUpFixture_Case([InjectDependency] SetUpFixture setUpFixture) =>
            setUpFixture.Should().NotBeNull();

        [TestCaseSource(nameof(InjectParentServiceCase))]
        [Repeat(2)]
        public void Inject_SetUpFixture_Service(SetUpFixture fixtureService, IMockService setUpFixtureService) =>
            fixtureService.Service.Should().Be(setUpFixtureService);

        private static readonly TestCaseData[] InjectParentServiceCase =
        {
            new(new InjectedService<SetUpFixture>(), new InjectLogFromSetUpFixture()),
        };

        private class InjectLogFromSetUpFixture : IInjectedService
        {
            public Type? GetServiceType(object? methodCallTargetFixture, object containerFixture) =>
                methodCallTargetFixture == containerFixture
                    ? null
                    : typeof(IMockService);
        }
    }
}
