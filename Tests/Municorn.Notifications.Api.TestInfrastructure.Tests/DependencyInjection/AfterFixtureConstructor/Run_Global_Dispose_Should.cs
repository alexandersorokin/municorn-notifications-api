using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Scopes.Inject;
using Municorn.Notifications.Api.TestInfrastructure.Logging;
using Municorn.Notifications.Api.TestInfrastructure.NUnitAttributes;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.AfterFixtureConstructor
{
    [TestFixture]
    internal sealed class Run_Global_Dispose_Should : ITestFixture, IDisposable
    {
        private readonly Counter counter = new();

        public void ConfigureServices(IServiceCollection serviceCollection) =>
            serviceCollection
                .AddContextualLog()
                .AddSingleton(this.counter)
                .AddSingleton<FixtureTimeLogger>();

        [Test]
        [Repeat(2)]
        public void Case([Inject] FixtureTimeLogger fixtureTimeLogger)
        {
            fixtureTimeLogger.Run();
            true.Should().BeTrue();
        }

        [CombinatorialTestCase(10)]
        [CombinatorialTestCase(11)]
        [Repeat(2)]
        public void Cases(int value, [Inject] FixtureTimeLogger fixtureTimeLogger)
        {
            fixtureTimeLogger.Run();
            value.Should().BePositive();
        }

        public void Dispose()
        {
            this.counter.Value.Should().Be(1);
        }
    }
}
