using System;
using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.BeforeFixtureConstructor;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.BeforeFixtureConstructor
{
    [TestFixtureInjectable]
    [LogModule]
    [AdHocModule(typeof(Counter))]
    [AdHocModule(typeof(FixtureTimeLogger))]
    [PrimaryConstructor]
    internal sealed partial class Run_Global_Dispose_Should : IDisposable
    {
        private readonly Counter counter;

        [OneTimeSetUp]
        public void OneTimeSetUp(FixtureTimeLogger fixtureTimeLogger)
        {
            fixtureTimeLogger.Run();
        }

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
