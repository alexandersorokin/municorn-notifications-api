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
            var (service, disposable) = SubstituteExtensions.For<IFixtureOneTimeSetUpService, IDisposable>();

            await CreateAndDisposeFramework(service).ConfigureAwait(false);

            disposable.Received().Dispose();
        }

        [Test]
        public async Task Run_DisposeAsync()
        {
            var (service, disposable) = SubstituteExtensions.For<IFixtureOneTimeSetUpService, IAsyncDisposable>();

            await CreateAndDisposeFramework(service).ConfigureAwait(false);

            await disposable.Received().DisposeAsync().ConfigureAwait(false);
        }

        [Test]
        public async Task Run_Only_DisposeAsync_If_Both_Interfaces_Are_Implemented()
        {
            var (service, disposable, asyncDisposable) = SubstituteExtensions.For<IFixtureOneTimeSetUpService, IDisposable, IAsyncDisposable>();

            await CreateAndDisposeFramework(service).ConfigureAwait(false);

            await asyncDisposable.Received().DisposeAsync().ConfigureAwait(false);
            disposable.DidNotReceive().Dispose();
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
