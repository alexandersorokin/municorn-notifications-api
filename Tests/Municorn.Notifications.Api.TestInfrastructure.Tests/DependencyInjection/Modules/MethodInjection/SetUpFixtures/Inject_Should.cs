using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Modules.MethodInjection.SetUpFixtures
{
    [TestFixture]
    internal class Inject_Should : FrameworkServiceProviderFixtureBase
    {
        public Inject_Should()
            : base(serviceCollection => serviceCollection
                .AddTestMethodInjection()
                .AddSingleton<MockService>())
        {
        }

        [Test]
        [Repeat(2)]
        public void Inject_Service([InjectParameterDependency] MockService service) => service.Should().NotBeNull();
    }
}
