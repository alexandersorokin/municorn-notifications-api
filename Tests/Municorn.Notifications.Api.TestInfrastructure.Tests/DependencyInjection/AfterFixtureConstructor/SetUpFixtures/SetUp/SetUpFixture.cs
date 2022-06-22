using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor;
using Municorn.Notifications.Api.TestInfrastructure.Logging;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.AfterFixtureConstructor.SetUpFixtures.SetUp
{
    [SetUpFixture]
    internal sealed class SetUpFixture : ITestFixture, IDisposable
    {
        [TestDependency]
        private readonly Counter counter = default!;

        public void ConfigureServices(IServiceCollection serviceCollection) =>
            serviceCollection
                .AddBoundLog()
                .AddTestTimeLogger();

        public void Dispose()
        {
            this.counter.Value.Should().Be(3);
        }
    }
}
