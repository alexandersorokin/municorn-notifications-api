using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Modules.FieldInjection
{
    internal class Inject_Field_Should : FrameworkServiceProviderFixtureBase
    {
        [InjectFieldDependency]
        private readonly MockService service = default!;

        public Inject_Field_Should()
            : base(serviceCollection => serviceCollection
                .AddFieldInjection(typeof(Inject_Field_Should))
                .AddSingleton<MockService>())
        {
        }

        [Test]
        public void Case() => this.service.Should().NotBeNull();

        [TestCase(10)]
        [TestCase(11)]
        public void Cases(int value) => this.service.Should().NotBeNull();
    }
}
