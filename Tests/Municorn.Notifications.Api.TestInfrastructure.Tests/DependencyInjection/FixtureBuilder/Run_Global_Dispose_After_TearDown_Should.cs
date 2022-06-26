using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureBuilder;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureBuilder
{
    [TestFixtureInjectable]
    [LogModule]
    [CounterModule]
    [FixtureModuleService(typeof(FixtureOneTimeTimeLogger))]
    [PrimaryConstructor]
    internal partial class Run_Global_Dispose_After_TearDown_Should
    {
        private readonly Counter counter;

        [OneTimeSetUp]
        public void OneTimeSetUp(FixtureOneTimeTimeLogger fixtureOneTimeTimeLogger)
        {
            fixtureOneTimeTimeLogger.Run();
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

        [OneTimeTearDown]
        public void Before_Container_Dispose()
        {
            this.counter.Value.Should().Be(0);
        }
    }
}
