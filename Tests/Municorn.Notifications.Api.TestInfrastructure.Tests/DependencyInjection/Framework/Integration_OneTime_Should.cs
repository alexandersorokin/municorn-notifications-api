using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Framework
{
    [TestFixture]
    internal sealed class Integration_OneTime_Should : IDisposable
    {
        private readonly Counter counter = new();
        private readonly FixtureServiceProviderFramework framework;

        public Integration_OneTime_Should() => this.framework = new(serviceCollection => serviceCollection
            .AddSingleton(this.counter)
            .AddSingleton<IFixtureOneTimeSetUpService, OneTimeIncrement>());

        [OneTimeSetUp]
        public async Task OneTimeSetUp() => await this.framework.RunOneTimeSetUp().ConfigureAwait(false);

        [OneTimeTearDown]
        public async Task OneTimeTearDown() => await this.framework.DisposeAsync().ConfigureAwait(false);

        [Test]
        [Repeat(2)]
        public void Test1() => this.counter.Value.Should().BePositive();

        [Test]
        public void Test() => this.counter.Value.Should().BePositive();

        public void Dispose()
        {
            const int oneTimeIncrementCount = 2;
            this.counter.Value.Should().Be(oneTimeIncrementCount);
        }

        private class OneTimeIncrement : IFixtureOneTimeSetUpService, IAsyncDisposable
        {
            private readonly Counter counter;

            public OneTimeIncrement(Counter counter) => this.counter = counter;

            public void Run() => this.counter.Increment();

            public ValueTask DisposeAsync()
            {
                this.counter.Increment();
                return ValueTask.CompletedTask;
            }
        }
    }
}
