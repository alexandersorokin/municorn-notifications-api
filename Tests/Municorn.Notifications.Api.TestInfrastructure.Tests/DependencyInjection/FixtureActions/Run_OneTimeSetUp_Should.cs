using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureActions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureActions
{
    [TestFixture]
    internal sealed class Run_OneTimeSetUp_Should : IFixtureWithServiceProviderFramework, IDisposable
    {
        private readonly Counter counter = new();

        public void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection
            .AddSingleton(this.counter)
            .AddSingleton<IFixtureOneTimeSetUpService, IncrementService>();

        [Test]
        [Repeat(2)]
        public void Case() => this.counter.Value.Should().Be(1);

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(2)]
        public void Cases(int value)
        {
            this.counter.Value.Should().Be(1);
            value.Should().BePositive();
        }

        public void Dispose() => this.counter.Value.Should().Be(1);

        private sealed class IncrementService : IFixtureOneTimeSetUpService
        {
            private readonly Counter counter;

            public IncrementService(Counter counter) => this.counter = counter;

            public void Run() => this.counter.Increment();
        }
    }
}
