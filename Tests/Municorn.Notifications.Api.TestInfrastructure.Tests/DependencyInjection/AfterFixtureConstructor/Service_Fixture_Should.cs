using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.ScopeAsyncLocal;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.ScopeMethodInject;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.AfterFixtureConstructor
{
    [TestFixture]
    internal class Service_Fixture_Should : IConfigureServices
    {
        public void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection.AddSingleton<object>();

        [Test]
        [Repeat(2)]
        public void Case([Inject] Service_Fixture_Should fixture)
        {
            fixture.Should().Be(this);
        }

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(2)]
        public void Cases(int value)
        {
            this.GetRequiredService<Service_Fixture_Should>().Should().Be(this);
        }
    }
}
