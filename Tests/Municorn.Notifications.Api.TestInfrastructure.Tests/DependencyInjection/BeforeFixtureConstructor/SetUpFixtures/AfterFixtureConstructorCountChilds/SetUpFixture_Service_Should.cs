using System;
using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.BeforeFixtureConstructor;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.ScopeMethodInject;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.BeforeFixtureConstructor.SetUpFixtures.AfterFixtureConstructorCountChilds
{
    [TestFixtureInjectable]
    [LogModule]
    internal class SetUpFixture_Service_Should
    {
        [Test]
        [Repeat(2)]
        public void Inject_Case([Inject] ILog service)
        {
            service.Should().NotBeNull();
        }

        [Test]
        [Repeat(2)]
        public void SetUpFixture_Case([Inject] SetUpFixture setUpFixture)
        {
            setUpFixture.Should().NotBeNull();
        }

        [TestCaseSource(nameof(InjectParentServiceCase))]
        [Repeat(2)]
        public void Inject_SetUpFixture_Service(SetUpFixture fixtureService, Counter setUpFixtureLog)
        {
            fixtureService.Counter.Should().Be(setUpFixtureLog);
        }

        private static readonly TestCaseData[] InjectParentServiceCase =
        {
            new(new InjectedService<SetUpFixture>(), new InjectLogFromSetUpFixture()),
        };

        private class InjectLogFromSetUpFixture : IInjectedService
        {
            public Type? GetServiceType(object? methodFixture, object containerFixture) =>
                methodFixture == containerFixture
                    ? null
                    : typeof(Counter);
        }
    }
}
