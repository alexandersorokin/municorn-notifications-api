using System;
using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureBuilder;
using Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureBuilder.Modules.ForFixtureBuilderOnly.ModuleService;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureBuilder
{
    [TestFixtureInjectable]
    [CounterModule]
    [FixtureModuleService(typeof(OnDisposeIncrementService))]
    [PrimaryConstructor]
    internal sealed partial class Run_Global_Dispose_Should : IDisposable
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

        public void Dispose() => this.counter.Value.Should().Be(1);

        internal sealed class OnDisposeIncrementService : IDisposable
        {
            private readonly Counter counter;

            public OnDisposeIncrementService(Counter counter) => this.counter = counter;

            public void Dispose() => this.counter.Increment();
        }
    }
}
