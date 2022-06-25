using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection;
using Municorn.Notifications.Api.TestInfrastructure.NUnitAttributes;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Modules.MethodInjection
{
    internal class Inject_To_Methods_Via_Extensions_Should : FrameworkServiceProviderFixtureBase
    {
        public Inject_To_Methods_Via_Extensions_Should()
            : base(serviceCollection => serviceCollection
                .AddTestMethodInjection()
                .AddSingleton(new SilentLog()))
        {
        }

        [Test]
        public void Simple_Inject([InjectDependency] SilentLog service) => service.Should().NotBeNull();

        [Test]
        [Repeat(3)]
        public void Repeat_Inject([InjectDependency] SilentLog service) => service.Should().NotBeNull();

        [CombinatorialTestCase(1)]
        [CombinatorialTestCase(2)]
        public void Case_Inject([InjectDependency] SilentLog service, int value)
        {
            service.Should().NotBeNull();
            value.Should().BePositive();
        }
    }
}
