using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.AfterFixtureConstructor.SetUpFixtures.Service
{
    [TestFixture]
    [TestMethodInjectionModule]
    internal class SetUpFixture_Inject_Should : ITestFixture
    {
        public void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection
            .AddScoped<ILog, SilentLog>();

        [Test]
        [Repeat(2)]
        public void Inject_Service([InjectDependency] ILog service)
        {
            service.Should().NotBeNull();
        }

        [Test]
        [Repeat(2)]
        public void Inject_Fixture([InjectDependency] SetUpFixture fixtureService)
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
