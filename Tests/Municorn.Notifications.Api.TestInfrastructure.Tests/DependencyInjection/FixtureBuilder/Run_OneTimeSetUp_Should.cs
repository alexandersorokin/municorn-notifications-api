using System;
using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureBuilder;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework;
using Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureBuilder.Modules.ForFixtureBuilderOnly.ModuleService;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureBuilder
{
    [TestFixtureInjectable]
    [CounterModule]
    [FixtureModuleService(typeof(IFixtureOneTimeSetUpService), typeof(IncrementService))]
    [PrimaryConstructor]
    internal sealed partial class Run_OneTimeSetUp_Should : IDisposable
    {
        private readonly Counter counter;

        [Test]
        [Repeat(2)]
        public void Case() => this.counter.Value.Should().Be(1);

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(2)]
        public void Cases(int value)
        {
            value.Should().BePositive();
            this.counter.Value.Should().Be(1);
        }

        public void Dispose() => this.counter.Value.Should().Be(1);

        internal sealed class IncrementService : IFixtureOneTimeSetUpService
        {
            private readonly Counter counter;

            public IncrementService(Counter counter) => this.counter = counter;

            public void Run() => this.counter.Increment();
        }
    }
}
