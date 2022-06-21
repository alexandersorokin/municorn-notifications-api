using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.Tests.DependencyInjection.ScopeMethodInject;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.AfterFixtureConstructor.Tests.SetUpFixtures.Service
{
    [TestFixture]
    internal class SetUpFixture_Inject_Should : IConfigureServices
    {
        public void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection
            .AddScoped<ILog, SilentLog>();

        [Test]
        [Repeat(2)]
        public void Inject_Service([Inject] ILog service)
        {
            service.Should().NotBeNull();
        }

        [Test]
        [Repeat(2)]
        public void Inject_Fixture([Inject] SetUpFixture fixtureService)
        {
            fixtureService.Service.Should().NotBeNull();
        }

        [TestCaseSource(nameof(InjectParentServiceCase))]
        [Repeat(2)]
        public void Inject_SetUpFixture_Service(SetUpFixture fixtureService, ILog setUpFixtureLog)
        {
            fixtureService.Service.Should().Be(setUpFixtureLog);
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
                    : typeof(ILog);
        }
    }
}
