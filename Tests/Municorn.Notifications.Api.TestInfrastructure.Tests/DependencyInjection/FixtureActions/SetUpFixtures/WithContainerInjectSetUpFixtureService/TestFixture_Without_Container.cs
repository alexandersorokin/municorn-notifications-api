using FluentAssertions;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureActions.SetUpFixtures.WithContainerInjectSetUpFixtureService
{
    [TestFixture]
    internal class TestFixture_Without_Container
    {
        [TestCaseSource(nameof(InjectParentServiceCase))]
        [Repeat(2)]
        public void Inject_SetUpFixture_Service_Without_Container_In_Current_Fixture(MockService setUpFixtureService) =>
            setUpFixtureService.Should().NotBeNull();

        private static readonly TestCaseData[] InjectParentServiceCase =
        {
            new(new InjectedServiceFromSetUpFixture()),
        };
    }
}
