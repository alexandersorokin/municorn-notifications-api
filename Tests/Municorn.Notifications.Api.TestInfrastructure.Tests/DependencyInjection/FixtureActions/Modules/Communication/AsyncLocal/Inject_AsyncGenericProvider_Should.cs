using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureActions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.TestCommunication;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureActions.Modules.Communication.AsyncLocal
{
    [TestFixture]
    internal class Inject_AsyncGenericProvider_Should : IFixtureWithServiceProviderFramework
    {
        [InjectFieldDependency]
        private readonly IAsyncLocalServiceProvider<MockService> service = default!;

        public void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection
            .AddTestCommunication()
            .AddFieldInjection(this)
            .AddScoped<MockService>();

        [SetUp]
        public void SetUp() => this.service.Value.Should().NotBeNull();

        [TearDown]
        public void TearDown() => this.service.Value.Should().NotBeNull();

        [Test]
        [Repeat(2)]
        public void Case() => this.service.Value.Should().NotBeNull();

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(2)]
        public void Cases(int value) => this.service.Value.Should().NotBeNull();
    }
}
