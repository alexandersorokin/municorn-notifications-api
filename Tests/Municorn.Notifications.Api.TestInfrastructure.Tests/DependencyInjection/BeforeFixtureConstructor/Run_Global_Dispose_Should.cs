using System;
using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureBuilder;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.BeforeFixtureConstructor
{
    [TestFixtureInjectable]
    [LogModule]
    [RegisterConstructorParametersModule]
    [FixtureModuleService(typeof(FixtureOneTimeTimeLogger))]
    internal sealed class Run_Global_Dispose_Should : IDisposable
    {
        private readonly Counter counter;

        public Run_Global_Dispose_Should([RegisterDependency] Counter counter) => this.counter = counter;

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

        public void Dispose()
        {
            this.counter.Value.Should().Be(1);
        }
    }
}
