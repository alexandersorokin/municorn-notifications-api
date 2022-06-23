using System;
using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.BeforeFixtureConstructor;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.BeforeFixtureConstructor
{
    [TestFixtureInjectable]
    [LogModule]
    [RegisterDependencyModule]
    [FixtureModule(typeof(IFixtureOneTimeSetUp), typeof(FixtureTimeLogger))]
    internal sealed class Run_OneTimeSetUp_Should : IDisposable
    {
        private readonly Counter counter;

        public Run_OneTimeSetUp_Should([RegisterDependency] Counter counter) => this.counter = counter;

        [Test]
        [Repeat(2)]
        public void Case()
        {
            true.Should().BeTrue();
        }

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(2)]
        public void Cases(int value)
        {
            value.Should().BePositive();
        }

        public void Dispose()
        {
            this.counter.Value.Should().Be(1);
        }
    }
}
