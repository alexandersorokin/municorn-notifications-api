using FluentAssertions;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureActions.SetUpFixtures.RunSetUpViaScopedService
{
    [TestFixture]
    internal class TestFixture_Without_Container
    {
        [Test]
        public void Work_Without_Container_In_Child_Fixture() => true.Should().BeTrue();
    }
}
