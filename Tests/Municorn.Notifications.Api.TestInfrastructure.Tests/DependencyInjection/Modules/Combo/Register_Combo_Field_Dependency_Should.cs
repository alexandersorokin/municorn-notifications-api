using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Combo;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Modules.Combo
{
    internal class Register_Combo_Field_Dependency_Should : FrameworkServiceProviderFixtureBase
    {
        [InjectFieldDependency]
        [RegisterDependency]
        private readonly MockService service = default!;

        public Register_Combo_Field_Dependency_Should()
            : base(serviceCollection => serviceCollection
                .AddFieldInjection(typeof(Register_Combo_Field_Dependency_Should)))
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
