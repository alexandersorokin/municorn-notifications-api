using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureActions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureActions.Modules.Attributes
{
    [TestFixture]
    [TestMethodInjectionModule]
    internal class Inject_Method_Should : IFixtureWithServiceProviderFramework
    {
        public void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection
            .AddScoped<MockService>();

        [Test]
        [Repeat(2)]
        public void Case([InjectDependency] MockService service) => service.Should().NotBeNull();
    }
}
