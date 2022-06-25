using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Framework
{
    [TestFixture]
    internal class Dispose_Scoped_Services_Should
    {
        [Test]
        public async Task Run_Dispose()
        {
            var service = Substitute.For<IFixtureSetUpService, IDisposable>();

            await CreateAndDisposeFrameworkScope(service).ConfigureAwait(false);

            (service as IDisposable)!.Received().Dispose();
        }

        [Test]
        public async Task Run_DisposeAsync()
        {
            var service = Substitute.For<IFixtureSetUpService, IAsyncDisposable>();

            await CreateAndDisposeFrameworkScope(service).ConfigureAwait(false);

            await (service as IAsyncDisposable)!.Received().DisposeAsync().ConfigureAwait(false);
        }

        [Test]
        public async Task Run_Only_DisposeAsync_If_Both_Interfaces_Are_Implemented()
        {
            var service = Substitute.For<IFixtureSetUpService, IAsyncDisposable, IDisposable>();

            await CreateAndDisposeFrameworkScope(service).ConfigureAwait(false);

            await (service as IAsyncDisposable)!.Received().DisposeAsync().ConfigureAwait(false);
            (service as IDisposable)!.DidNotReceive().Dispose();
        }

        private static async Task CreateAndDisposeFrameworkScope(IFixtureSetUpService service)
        {
            FixtureServiceProviderFramework framework = new(serviceCollection => serviceCollection
                .AddScoped(_ => service));

            await using (framework.ConfigureAwait(false))
            {
                var currentTest = TestExecutionContext.CurrentContext.CurrentTest;
                await framework.RunSetUp(currentTest).ConfigureAwait(false);

                await framework.RunTearDown(currentTest).ConfigureAwait(false);
            }
        }
    }
}
