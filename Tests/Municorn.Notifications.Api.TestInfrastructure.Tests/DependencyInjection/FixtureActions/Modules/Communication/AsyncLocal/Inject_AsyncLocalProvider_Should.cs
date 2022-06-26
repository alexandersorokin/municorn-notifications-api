using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.TestCommunication;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureActions.Modules.Communication.AsyncLocal
{
    [TestFixture]
    internal class Inject_AsyncLocalProvider_Should : IWithFields
    {
        [FieldDependency]
        private readonly IAsyncLocalServiceProvider provider = default!;

        public void SetUpServices(IServiceCollection serviceCollection) => serviceCollection
            .AddTestCommunication()
            .AddScoped<MockService>();

        [SetUp]
        public void SetUp() => this.provider.GetRequiredService<MockService>().Should().NotBeNull();

        [TearDown]
        public void TearDown() => this.provider.GetRequiredService<MockService>().Should().NotBeNull();

        [Test]
        [Repeat(2)]
        public void Case() => this.provider.GetRequiredService<MockService>().Should().NotBeNull();

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(2)]
        public void Cases(int value) => this.provider.GetRequiredService<MockService>().Should().NotBeNull();
    }
}
