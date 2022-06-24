using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.BeforeFixtureConstructor;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.BeforeFixtureConstructor.SetUpFixtures.Empty
{
    [TestFixtureInjectable]
    [TestMethodInjectionModule]
    [LogModule]
    internal class SetUpFixture_Service_Should
    {
        [Test]
        [Repeat(2)]
        public void Inject_Case([InjectDependency] ILog service)
        {
            service.Should().NotBeNull();
        }

        [Test]
        [Repeat(2)]
        public void SetUpFixture_Case([InjectDependency] SetUpFixture setUpFixture)
        {
            setUpFixture.Should().NotBeNull();
        }
    }
}
