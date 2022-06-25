using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureBuilder;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Abstractions;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.BeforeFixtureConstructor
{
    [TestFixtureInjectable]
    [LogModule]
    [RegisterConstructorParametersModule]
    [FixtureModuleService(typeof(FixtureOneTimeTimeLogger))]
    internal class Run_Global_Dispose_After_TearDown_Should
    {
        private readonly Counter counter;

        public Run_Global_Dispose_After_TearDown_Should([RegisterDependency] Counter counter) => this.counter = counter;

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
