using FluentAssertions;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureActions.SetUpFixtures.Service
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
