using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Abstractions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Modules.FieldInjection
{
    [FieldInjectionModule]
    internal class Attribute_Should : FrameworkServiceProviderFixtureBase
    {
        [FieldDependency]
        private readonly SilentLog service = default!;

        public Attribute_Should()
            : base(serviceCollection => serviceCollection
                .AddFixtureServiceCollectionModuleAttributes(typeof(Attribute_Should))
                .AddSingleton<SilentLog>())
        {
        }

        [Test]
        public void Case() => this.service.Should().NotBeNull();
    }
}
