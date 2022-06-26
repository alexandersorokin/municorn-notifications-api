using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureBuilder;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureBuilder
{
    [TestFixtureInjectable]
    [MockServiceModule]
    internal class Fixture_Methods_Inject_Should
    {
        [OneTimeSetUp]
        public void OneTimeSetUp(MockService service) => service.Should().NotBeNull();

        [OneTimeTearDown]
        public void OneTimeTearDown(MockService service) => service.Should().NotBeNull();

        [SetUp]
        public void SetUp(MockService service) => service.Should().NotBeNull();

        [TearDown]
        public void TearDown(MockService service) => service.Should().NotBeNull();

        [Test]
        [Repeat(2)]
        public void Case() => true.Should().BeTrue();

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(2)]
        public void Cases(int value) => true.Should().BeTrue();
    }
}
