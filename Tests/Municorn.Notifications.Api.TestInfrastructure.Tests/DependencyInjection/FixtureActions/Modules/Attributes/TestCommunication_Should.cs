using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureActions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.TestCommunication;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureActions.Modules.Attributes
{
    [TestFixture]
    [TestCommunicationModule]
    internal class TestCommunication_Should : IFixtureWithServiceProviderFramework
    {
        public void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection
            .AddScoped<MockService>();

        [Test]
        [Repeat(2)]
        public void Case() => this.GetRequiredService<MockService>().Should().NotBeNull();
    }
}
