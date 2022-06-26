using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureActions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureActions.SetUpFixtures.Empty
{
    [TestFixture]
    internal class SetUpFixture_Inject_Should : IFixtureWithServiceProviderFramework
    {
        public void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection
            .AddTestMethodInjection()
            .AddScoped<MockService>();

        [Test]
        [Repeat(2)]
        public void Inject_Service([InjectDependency] MockService service) => service.Should().NotBeNull();

        [Test]
        [Repeat(2)]
        public void Inject_Fixture([InjectDependency] SetUpFixture fixtureService) => fixtureService.Should().NotBeNull();
    }
}
