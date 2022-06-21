using FluentAssertions;
using Municorn.Notifications.Api.Tests.DependencyInjection.AutoMethods;
using Municorn.Notifications.Api.Tests.DependencyInjection.ScopeMethodInject;
using Municorn.Notifications.Api.Tests.NUnitAttributes;
using NUnit.Framework;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.BeforeFixtureConstructor.Tests
{
    [TestFixtureInjectable]
    [TestModule]
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
