using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Modules.MethodInjection
{
    internal class Inject_Scoped_Services_Should : FrameworkServiceProviderFixtureBase
    {
        public Inject_Scoped_Services_Should()
            : base(serviceCollection => serviceCollection
                .AddTestMethodInjection()
                .AddScoped<MockService>())
        {
        }

        [Test]
        public void Simple_Inject([InjectDependency] MockService service) => service.Should().NotBeNull();

        [Test]
        [Repeat(3)]
        public void Repeat_Inject([InjectDependency] MockService service) => service.Should().NotBeNull();
    }
}
