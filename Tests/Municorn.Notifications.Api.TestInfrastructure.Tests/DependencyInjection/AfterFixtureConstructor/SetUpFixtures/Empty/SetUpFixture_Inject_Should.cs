﻿using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.ScopeMethodInject;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.AfterFixtureConstructor.SetUpFixtures.Empty
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
            fixtureService.Should().NotBeNull();
        }
    }
}