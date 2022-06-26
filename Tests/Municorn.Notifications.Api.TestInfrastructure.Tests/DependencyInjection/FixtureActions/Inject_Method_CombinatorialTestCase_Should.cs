using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureActions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection;
using Municorn.Notifications.Api.TestInfrastructure.NUnitAttributes;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureActions
{
    [TestFixture]
    internal class Inject_Method_CombinatorialTestCase_Should : IFixtureWithServiceProviderFramework
    {
        public void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection
            .AddTestMethodInjection()
            .AddScoped<MockService>();

        [CombinatorialTestCase]
        [Repeat(2)]
        public void NoParams() => true.Should().BeTrue();

        [CombinatorialTestCase]
        [Repeat(2)]
        public void Case([InjectDependency] MockService service) => service.Should().NotBeNull();

        [CombinatorialTestCase(10)]
        [CombinatorialTestCase(11)]
        [Repeat(2)]
        public void Cases(int value, [InjectDependency] MockService service) => service.Should().NotBeNull();
    }
}
