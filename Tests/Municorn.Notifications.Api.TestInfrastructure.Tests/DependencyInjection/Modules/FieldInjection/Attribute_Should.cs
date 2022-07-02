using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Abstractions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Modules.FieldInjection
{
    [FieldInjectionModule]
    internal class Attribute_Should : FrameworkServiceProviderFixtureBase
    {
        [InjectFieldDependency]
        private readonly MockService service = default!;

        public Attribute_Should()
            : base(serviceCollection => serviceCollection
                .AddFixtureServiceCollectionModuleAttributes(typeof(Attribute_Should))
                .AddSingleton<MockService>())
        {
        }

        [Test]
        public void Case() => this.service.Should().NotBeNull();
    }
}
