using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureActions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureActions.Modules.Fields
{
    [TestFixture]
    internal class Register_Field_Dependency_ImplementationType_Should : IFixtureWithServiceProviderFramework
    {
        [FieldDependency]
        [RegisterFieldDependency(typeof(MockService))]
        private readonly IMockService service = default!;

        public void ConfigureServices(IServiceCollection serviceCollection) =>
            serviceCollection.AddFieldInjection(this);

        [Test]
        [Repeat(2)]
        public void Case() => this.service.Should().NotBeNull();

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(2)]
        public void Cases(int value) => this.service.Should().NotBeNull();
    }
}
