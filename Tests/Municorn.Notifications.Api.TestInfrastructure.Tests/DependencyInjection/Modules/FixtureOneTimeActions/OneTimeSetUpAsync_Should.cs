using System.Threading.Tasks;
using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FixtureOneTimeActions;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Modules.FixtureOneTimeActions
{
    internal class OneTimeSetUpAsync_Should : FrameworkServiceProviderFixtureBase, IOneTimeSetUpAsyncAction
    {
        private readonly Counter counter = new();

        public OneTimeSetUpAsync_Should()
            : base(serviceCollection => serviceCollection.AddFixtureOneTimeActions())
        {
        }

        public Task OneTimeSetUpAsync()
        {
            this.counter.Increment();
            return Task.CompletedTask;
        }

        [Test]
        public void Plain_Test() => this.counter.Value.Should().Be(1);

        [Test]
        [Repeat(3)]
        public void Test_With_Repeat() => this.counter.Value.Should().Be(1);

        [TestCase(1)]
        [TestCase(2)]
        public void TestCases(int value) => this.counter.Value.Should().Be(1);
    }
}
