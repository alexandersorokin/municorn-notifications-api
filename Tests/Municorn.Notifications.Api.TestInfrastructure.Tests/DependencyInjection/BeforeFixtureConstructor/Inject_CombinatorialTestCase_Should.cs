using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AutoMethods;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.BeforeFixtureConstructor;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.ScopeMethodInject;
using Municorn.Notifications.Api.TestInfrastructure.NUnitAttributes;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.BeforeFixtureConstructor
{
    [TestFixtureInjectable]
    [TestTimeLoggerModule]
    internal class Inject_CombinatorialTestCase_Should
    {
        [Test]
        [Repeat(2)]
        public void Case([Inject] IFixtureSetUp service)
        {
            service.Should().NotBeNull();
        }

        [CombinatorialTestCase(10)]
        [CombinatorialTestCase(11)]
        [Repeat(2)]
        public void Cases([Inject] IFixtureSetUp service, int value)
        {
            service.Should().NotBeNull();
        }

        [CombinatorialTestCaseSource(nameof(CaseValues))]
        [Repeat(2)]
        public void CaseSource(int value, [Inject] IFixtureSetUp service)
        {
            service.Should().NotBeNull();
        }

        private static readonly TestCaseData[] CaseValues =
        {
            new(10),
            new(11),
        };
    }
}
