using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Framework
{
    [TestFixture]
    internal class Framework_OneTimeSetUp_Should
    {
        [Test]
        public async Task Run()
        {
            Service service = new();
            var framework = new FixtureServiceProviderFramework(serviceCollection => serviceCollection
                .AddSingleton<IFixtureOneTimeSetUpService>(service));
            await using (framework.ConfigureAwait(false))
            {
                await framework.BeforeTestSuite().ConfigureAwait(false);
            }

            service.Called.Should().BeTrue();
        }

        private class Service : IFixtureOneTimeSetUpService
        {
            internal bool Called { get; private set; }

            public void Run() => this.Called = true;
        }
    }
}
