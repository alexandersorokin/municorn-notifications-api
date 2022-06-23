using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection;
using Municorn.Notifications.Api.TestInfrastructure.NUnitAttributes;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.AfterFixtureConstructor
{
    [TestFixture]
    [TestMethodInjectionModule]
    internal class Inject_Method_CombinatorialTestCase_Should : ITestFixture
    {
        public void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection
            .AddScoped<ILog, SilentLog>();

        [CombinatorialTestCase]
        [Repeat(2)]
        public void NoParams()
        {
            true.Should().BeTrue();
        }

        [CombinatorialTestCase]
        [Repeat(2)]
        public void Case([InjectDependency] ILog service)
        {
            service.Should().NotBeNull();
        }

        [CombinatorialTestCase(10)]
        [CombinatorialTestCase(11)]
        [Repeat(2)]
        public void Cases(int value, [InjectDependency] ILog service)
        {
            service.Should().NotBeNull();
        }
    }
}
