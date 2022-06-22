using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.BeforeFixtureConstructor;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.BeforeFixtureConstructor
{
    [TestFixtureInjectable]
    [FixtureModule(typeof(Counter))]
    internal class Fixture_Methods_Inject_Should
    {
        [OneTimeSetUp]
        public void OneTimeSetUp(Counter service)
        {
            service.Should().NotBeNull();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown(Counter service)
        {
            service.Should().NotBeNull();
        }

        [SetUp]
        public void SetUp(Counter service)
        {
            service.Should().NotBeNull();
        }

        [TearDown]
        public void TearDown(Counter service)
        {
            service.Should().NotBeNull();
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
            true.Should().BeTrue();
        }
    }
}
