using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureBuilder;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureBuilder.Modules.FoAll.FieldInjection
{
    [TestFixtureInjectable]
    [FieldInjectionModule]
    [PrimaryConstructor]
    internal partial class Register_Field_Dependency_ImplementationType_Should
    {
        [InjectFieldDependency]
        [RegisterFieldDependencyAsSingleton(typeof(MockService))]
        private readonly IMockService service;

        [Test]
        [Repeat(2)]
        public void Case() => this.service.Should().NotBeNull();

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(2)]
        public void Cases(int value) => this.service.Should().NotBeNull();
    }
}
