using FluentAssertions;
using NUnit.Framework;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.AfterFixtureConstructor.Tests.SetUpFixtures.OneTimeSetUp
{
    [TestFixture]
    internal class SetUpFixture_Should
    {
        [Test]
        public void Work_Without_Container_In_Child_Fixture()
        {
            true.Should().BeTrue();
        }
    }
}
