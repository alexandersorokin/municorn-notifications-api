using FluentAssertions;
using Municorn.Notifications.Api.Tests.DependencyInjection.AutoMethods;
using Municorn.Notifications.Api.Tests.DependencyInjection.ScopeMethodInject;
using NUnit.Framework;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.BeforeFixtureConstructor.Tests
{
    [TestFixtureInjectable]
    [TestModule]
    internal class Inject_Method_Should
    {
        [Test]
        [Repeat(2)]
        public void Case([Inject] IFixtureSetUp service)
        {
            service.Should().NotBeNull();
        }

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(2)]
        public void Cases([Inject] IFixtureSetUp service, int value)
        {
            service.Should().NotBeNull();
        }

        [TestCaseSource(nameof(CaseValues))]
        [Repeat(2)]
        public void CaseSource([Inject] IFixtureSetUp service, int value)
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
