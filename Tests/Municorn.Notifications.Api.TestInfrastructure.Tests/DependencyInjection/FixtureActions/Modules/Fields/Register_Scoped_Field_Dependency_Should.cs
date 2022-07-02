using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureActions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.TestCommunication;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureActions.Modules.Fields
{
    [TestFixture]
    internal class Register_Scoped_Field_Dependency_Should : IFixtureWithServiceProviderFramework
    {
        [FieldDependency]
        [RegisterDependency]
        private readonly IAsyncLocalServiceProvider<MockService> service = default!;

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
