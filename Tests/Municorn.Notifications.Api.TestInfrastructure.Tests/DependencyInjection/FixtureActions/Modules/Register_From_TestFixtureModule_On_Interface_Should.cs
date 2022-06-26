using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureActions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureActions.Modules
{
    [TestFixture]
    internal class Register_From_TestFixtureModule_On_Interface_Should : IFixtureWithServiceProviderFramework
    {
        public void ConfigureServices(IServiceCollection serviceCollection) =>
            serviceCollection
                .AddTestMethodInjection()
                .AddSingleton<MockService>();

        [Test]
        [Repeat(2)]
        public void Case([InjectDependency] MockService service) => service.Should().NotBeNull();
    }
}
