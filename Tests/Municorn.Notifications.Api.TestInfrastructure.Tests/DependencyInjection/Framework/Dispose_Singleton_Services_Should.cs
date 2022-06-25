using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework;
using NSubstitute;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Framework
{
    [TestFixture]
    internal class Dispose_Singleton_Services_Should
    {
        [Test]
        public async Task Run_Dispose()
        {
            var service = Substitute.For<IFixtureOneTimeSetUpService, IDisposable>();

            await CreateAndDisposeFramework(service).ConfigureAwait(false);

            ((IDisposable)service).Received().Dispose();
        }

        [Test]
        public async Task Run_DisposeAsync()
        {
            var service = Substitute.For<IFixtureOneTimeSetUpService, IAsyncDisposable>();

            await CreateAndDisposeFramework(service).ConfigureAwait(false);

            await ((IAsyncDisposable)service).Received().DisposeAsync().ConfigureAwait(false);
        }

        [Test]
        public async Task Run_Only_DisposeAsync_If_Both_Interfaces_Are_Implemented()
        {
            var service = Substitute.For<IFixtureOneTimeSetUpService, IDisposable, IAsyncDisposable>();

            await CreateAndDisposeFramework(service).ConfigureAwait(false);

            await ((IAsyncDisposable)service).Received().DisposeAsync().ConfigureAwait(false);
            ((IDisposable)service).DidNotReceive().Dispose();
        }

        private static async Task CreateAndDisposeFramework(IFixtureOneTimeSetUpService service)
        {
            FixtureServiceProviderFramework framework = new(serviceCollection => serviceCollection
                .AddSingleton(_ => service));

            await using (framework.ConfigureAwait(false))
            {
                await framework.RunOneTimeSetUp().ConfigureAwait(false);
            }
        }
    }
}
