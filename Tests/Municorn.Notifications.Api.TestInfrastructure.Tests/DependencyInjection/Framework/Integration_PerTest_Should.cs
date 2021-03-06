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
    internal sealed class Integration_PerTest_Should : IDisposable
    {
        private const int RepeatCount = 3;

        private readonly Counter counter = new();
        private readonly FixtureServiceProviderFramework framework;

        public Integration_PerTest_Should() => this.framework = new(serviceCollection => serviceCollection
            .AddSingleton(this.counter)
            .AddScoped<IFixtureSetUpService, Increment>());

        private static Test CurrentTest => TestExecutionContext.CurrentContext.CurrentTest;

        [SetUp]
        public async Task SetUp() => await this.framework.RunSetUp(CurrentTest).ConfigureAwait(false);

        [TearDown]
        public async Task TearDown() => await this.framework.RunTearDown(CurrentTest).ConfigureAwait(false);

        [Test]
        [Repeat(RepeatCount)]
        public void Test1() => this.counter.Value.Should().BePositive();

        [Test]
        public void Test2() => this.counter.Value.Should().BePositive();

        public void Dispose()
        {
            const int perTestIncrementCount = 2;

            const int notRepeatedTests = 1;
            const int repeatedTests = 1;
            const int repeatedTestRuns = repeatedTests * RepeatCount;
            this.counter.Value.Should().Be(perTestIncrementCount * (notRepeatedTests + repeatedTestRuns));
        }

        private class Increment : IFixtureSetUpService, IAsyncDisposable
        {
            private readonly Counter counter;

            public Increment(Counter counter) => this.counter = counter;

            public void Run() => this.counter.Increment();

            public ValueTask DisposeAsync()
            {
                this.counter.Increment();
                return ValueTask.CompletedTask;
            }
        }
    }
}
