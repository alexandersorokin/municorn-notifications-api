using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Framework
{
    [TestFixture]
    internal class Framework_OneTimeSetUp_Dispose_Should
    {
        [Test]
        public async Task Run()
        {
            Service service = new();
            FixtureServiceProviderFramework framework = new(serviceCollection => serviceCollection
                .AddSingleton<IFixtureOneTimeSetUpService>(_ => service));
            await using (framework.ConfigureAwait(false))
            {
                await framework.BeforeTestSuite().ConfigureAwait(false);
            }

            service.Called.Should().BeTrue();
        }

        private sealed class Service : IFixtureOneTimeSetUpService, IDisposable
        {
            internal bool Called { get; private set; }

            public void Run()
            {
                // Just to be resolved to dispose
            }

            public void Dispose() => this.Called = true;
        }
    }
}
