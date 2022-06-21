using FluentAssertions;
using Municorn.Notifications.Api.Tests.DependencyInjection.ScopeMethodInject;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.BeforeFixtureConstructor.Tests.SetUpFixtures
{
    [TestFixtureInjectable]
    [TestModule]
    internal class SetUpFixture_Service_Should
    {
        [Test]
        [Repeat(2)]
        public void Inject_Case([Inject] ILog service)
        {
            service.Should().NotBeNull();
        }

        [Test]
        [Repeat(2)]
        public void SetUpFixture_Case([Inject] SetUpFixture setUpFixture)
        {
            setUpFixture.Should().NotBeNull();
        }
    }
}
