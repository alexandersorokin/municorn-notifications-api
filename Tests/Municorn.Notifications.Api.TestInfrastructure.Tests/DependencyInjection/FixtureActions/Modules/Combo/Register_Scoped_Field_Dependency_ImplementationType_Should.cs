using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureActions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Combo;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.TestCommunication;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureActions.Modules.Combo
{
    [TestFixture]
    internal class Register_Scoped_Field_Dependency_ImplementationType_Should : IFixtureWithServiceProviderFramework
    {
        [InjectFieldDependency]
        [RegisterDependency(typeof(MockService))]
        private readonly IAsyncLocalServiceProvider<IMockService> service = default!;

        public void ConfigureServices(IServiceCollection serviceCollection) =>
            serviceCollection
                .AddTestCommunication()
                .AddFieldInjection(this);

        [Test]
        [Repeat(2)]
        public void Case() => this.service.Value.Should().NotBeNull();

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(2)]
        public void Cases(int value) => this.service.Value.Should().NotBeNull();
    }
}
