using FluentAssertions;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.Logging.FixtureTime.SetUpFixture.ContextualLogGoesToSetUpFixture
{
    [TestFixture]
    internal class TestFixture
    {
        [Test]
        public void Test() => true.Should().BeTrue();

        [TestCase(1)]
        [TestCase(2)]
        public void TestCase(int value) => value.Should().BePositive();
    }
}
