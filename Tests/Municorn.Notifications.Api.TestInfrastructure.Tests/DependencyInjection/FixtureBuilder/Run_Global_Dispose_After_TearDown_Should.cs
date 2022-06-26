using System;
using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureBuilder;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureBuilder
{
    [TestFixtureInjectable]
    [CounterModule]
    [FixtureModuleService(typeof(OnDisposeIncrementService))]
    [PrimaryConstructor]
    internal partial class Run_Global_Dispose_After_TearDown_Should
    {
        private readonly Counter counter;

        [OneTimeSetUp]
        public void OneTimeSetUp(OnDisposeIncrementService incrementService)
        {
            incrementService.Should().NotBeNull();
            this.counter.Value.Should().Be(0);
        }

        [Test]
        [Repeat(2)]
        public void Case() => this.counter.Value.Should().Be(0);

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(2)]
        public void Cases(int value)
        {
            value.Should().BePositive();
            this.counter.Value.Should().Be(0);
        }

        [OneTimeTearDown]
        public void Before_Container_Dispose() => this.counter.Value.Should().Be(0);

        internal sealed class OnDisposeIncrementService : IDisposable
        {
            private readonly Counter counter;

            public OnDisposeIncrementService(Counter counter) => this.counter = counter;

            public void Dispose() => this.counter.Increment();
        }
    }
}
