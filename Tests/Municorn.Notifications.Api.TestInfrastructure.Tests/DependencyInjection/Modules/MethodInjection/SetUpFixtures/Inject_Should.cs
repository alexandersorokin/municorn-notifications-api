using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Modules.MethodInjection.SetUpFixtures
{
    [TestFixture]
    internal class Inject_Should : FrameworkServiceProviderFixtureBase
    {
        public Inject_Should()
            : base(serviceCollection => serviceCollection
                .AddTestMethodInjection()
                .AddSingleton(new SilentLog()))
        {
        }

        [Test]
        [Repeat(2)]
        public void Inject_Service([InjectDependency] SilentLog service) => service.Should().NotBeNull();
    }
}
