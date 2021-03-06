using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Modules.FieldInjection
{
    internal class Register_Field_Dependency_Should : FrameworkServiceProviderFixtureBase
    {
        [InjectFieldDependency]
        [RegisterFieldDependencyAsSingleton]
        private readonly MockService service = default!;

        public Register_Field_Dependency_Should()
            : base(serviceCollection => serviceCollection
                .AddFieldInjection(typeof(Register_Field_Dependency_Should)))
        {
        }

        [Test]
        [Repeat(2)]
        public void Case() => this.service.Should().NotBeNull();

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(2)]
        public void Cases(int value) => this.service.Should().NotBeNull();
    }
}
