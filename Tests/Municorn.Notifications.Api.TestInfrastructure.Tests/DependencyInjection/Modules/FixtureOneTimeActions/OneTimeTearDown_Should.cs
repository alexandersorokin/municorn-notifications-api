using System;
using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FixtureOneTimeActions;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Modules.FixtureOneTimeActions
{
    internal sealed class OneTimeTearDown_Should : FrameworkServiceProviderFixtureBase, IOneTimeTearDownAction, IDisposable
    {
        private readonly Counter counter = new();

        public OneTimeTearDown_Should()
            : base(serviceCollection => serviceCollection.AddFixtureOneTimeActions())
        {
        }

        public void OneTimeTearDown() => this.counter.Increment();

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
