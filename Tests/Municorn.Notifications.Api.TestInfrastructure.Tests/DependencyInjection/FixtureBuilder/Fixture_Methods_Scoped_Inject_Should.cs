using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureBuilder;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureBuilder
{
    [TestFixtureInjectable]
    [TimeLoggerModule]
    internal class Fixture_Methods_Scoped_Inject_Should
    {
        [SetUp]
        public void SetUp(IFixtureSetUpService service)
        {
            service.Should().NotBeNull();
        }

        [TearDown]
        public void TearDown(IFixtureSetUpService service)
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
