using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Framework
{
    [TestFixture]
    internal sealed class Integration_All_Should : IDisposable
    {
        private const int RepeatCount = 3;
        private const int OneTimeIncrementCount = 2;
        private const int PerTestIncrementCount = 2;

        private readonly Counter counter = new();
        private readonly FixtureServiceProviderFramework framework;

        public Integration_All_Should() => this.framework = new(serviceCollection => serviceCollection
            .AddSingleton(this.counter)
            .AddSingleton<IFixtureOneTimeSetUpService, OneTimeIncrement>()
            .AddScoped<IFixtureSetUpService, Increment>());

        private static Test CurrentTest => TestExecutionContext.CurrentContext.CurrentTest;

        [OneTimeSetUp]
        public async Task OneTimeSetUp() => await this.framework.RunOneTimeSetUp().ConfigureAwait(false);

        [SetUp]
        public async Task SetUp() => await this.framework.RunSetUp(CurrentTest).ConfigureAwait(false);

        [TearDown]
        public async Task TearDown() => await this.framework.RunTearDown(CurrentTest).ConfigureAwait(false);

        [OneTimeTearDown]
        public async Task OneTimeTearDown() => await this.framework.DisposeAsync().ConfigureAwait(false);

        [Test]
        [Repeat(RepeatCount)]
        public void Test1()
        {
            this.counter.Value.Should().BePositive();
        }

        [Test]
        public void Test2()
        {
            this.counter.Value.Should().BePositive();
        }

        public void Dispose() => this.counter.Value.Should().Be(OneTimeIncrementCount + (PerTestIncrementCount * (1 + (1 * RepeatCount))));

        private sealed class Increment : IFixtureSetUpService, IDisposable
        {
            private readonly Counter counter;

            public Increment(Counter counter) => this.counter = counter;

            public void Run() => this.counter.Increment();

            public void Dispose() => this.counter.Increment();
        }

        private sealed class OneTimeIncrement : IFixtureOneTimeSetUpService, IDisposable
        {
            private readonly Counter counter;

            public OneTimeIncrement(Counter counter) => this.counter = counter;

            public void Run() => this.counter.Increment();

            public void Dispose() => this.counter.Increment();
        }
    }
}
