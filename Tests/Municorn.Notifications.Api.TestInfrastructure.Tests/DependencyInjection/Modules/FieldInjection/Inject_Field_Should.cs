using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Modules.FieldInjection
{
    internal class Inject_Field_Should : FrameworkServiceProviderFixtureBase
    {
        [FieldDependency]
        private readonly SilentLog service = default!;

        public Inject_Field_Should()
            : base(serviceCollection => serviceCollection
                .AddFieldInjection(typeof(Inject_Field_Should))
                .AddSingleton<SilentLog>())
        {
        }

        [Test]
        public void Case() => this.service.Should().NotBeNull();

        [TestCase(10)]
        [TestCase(11)]
        public void Cases(int value) => this.service.Should().NotBeNull();
    }
}
