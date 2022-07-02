using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureBuilder;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureBuilder.SetUpFixtures.Empty
{
    [TestFixtureInjectable]
    [TestMethodInjectionModule]
    [MockServiceModule]
    internal class SetUpFixture_Service_Should
    {
        [Test]
        [Repeat(2)]
        public void Inject_Case([InjectParameterDependency] MockService service) => service.Should().NotBeNull();

        [Test]
        [Repeat(2)]
        public void SetUpFixture_Case([InjectParameterDependency] SetUpFixture setUpFixture) => setUpFixture.Should().NotBeNull();
    }
}
