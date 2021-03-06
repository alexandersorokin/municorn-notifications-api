using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureBuilder;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Combo;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.TestCommunication;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureBuilder.Modules.FoAll.Combo
{
    [TestFixtureInjectable]
    [FieldInjectionModule]
    [TestCommunicationModule]
    [PrimaryConstructor]
    internal partial class Register_Scoped_Field_Dependency_ImplementationType_Should
    {
        [RegisterDependency(typeof(MockService))]
        private readonly IAsyncLocalServiceProvider<IMockService> service;

        [Test]
        [Repeat(2)]
        public void Case() => this.service.Value.Should().NotBeNull();

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(2)]
        public void Cases(int value) => this.service.Value.Should().NotBeNull();
    }
}
