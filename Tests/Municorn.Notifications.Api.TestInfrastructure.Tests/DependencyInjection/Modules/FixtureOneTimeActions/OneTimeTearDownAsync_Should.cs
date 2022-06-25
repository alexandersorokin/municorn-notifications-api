using System;
using System.Threading.Tasks;
using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FixtureOneTimeActions;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Modules.FixtureOneTimeActions
{
    internal sealed class OneTimeTearDownAsync_Should : FrameworkServiceProviderFixtureBase, IOneTimeTearDownAsyncAction, IDisposable
    {
        private readonly Counter counter = new();

        public OneTimeTearDownAsync_Should()
            : base(serviceCollection => serviceCollection.AddFixtureOneTimeActions())
        {
        }

        public Task OneTimeTearDownAsync()
        {
            this.counter.Increment();
            return Task.CompletedTask;
        }

        [Test]
        public void Plain_Test() => this.counter.Value.Should().Be(0);

        [Test]
        [Repeat(3)]
        public void Test_With_Repeat() => this.counter.Value.Should().Be(0);

        [TestCase(1)]
        [TestCase(2)]
        public void TestCases(int value) => this.counter.Value.Should().Be(0);

        public void Dispose() => this.counter.Value.Should().Be(1);
    }
}
