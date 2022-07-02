using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureBuilder;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureBuilder.Modules.ForFixtureBuilderOnly.RegisterConstructorParameters
{
    [TestFixtureInjectable]
    [RegisterConstructorParametersModule]
    internal class Use_Register_Parameters_Module_Should
    {
        private readonly MockService service;

        public Use_Register_Parameters_Module_Should([RegisterDependency] MockService service) =>
            this.service = service;

        [Test]
        [Repeat(2)]
        public void Case() => this.service.Should().NotBeNull();

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(2)]
        public void Cases(int value) => this.service.Should().NotBeNull();
    }
}
