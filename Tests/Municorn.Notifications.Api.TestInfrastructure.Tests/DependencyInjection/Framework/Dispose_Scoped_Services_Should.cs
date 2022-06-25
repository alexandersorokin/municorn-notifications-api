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
        public async Task Run_Dispose_Twice_For_Two_Finished_Scopes()
        {
            var (service, disposable) = SubstituteExtensions.For<IFixtureSetUpService, IDisposable>();

            var framework = CreateFramework(service);

            await using (framework.ConfigureAwait(false))
            {
                var currentTest = TestExecutionContext.CurrentContext.CurrentTest;
                var parentTest = currentTest.Parent!;
                await framework.RunSetUp(currentTest).ConfigureAwait(false);
                await framework.RunSetUp(parentTest).ConfigureAwait(false);

                await framework.RunTearDown(currentTest).ConfigureAwait(false);
                await framework.RunTearDown(parentTest).ConfigureAwait(false);
            }

            disposable.Received(2).Dispose();
        }

        [Test]
        public async Task Run_Dispose_Once_For_Finished_And_Unfinished_Scopes()
        {
            var (service, disposable) = SubstituteExtensions.For<IFixtureSetUpService, IDisposable>();

            var framework = CreateFramework(service);

            await using (framework.ConfigureAwait(false))
            {
                var currentTest = TestExecutionContext.CurrentContext.CurrentTest;
                var parentTest = currentTest.Parent!;
                await framework.RunSetUp(currentTest).ConfigureAwait(false);
                await framework.RunSetUp(parentTest).ConfigureAwait(false);

                await framework.RunTearDown(currentTest).ConfigureAwait(false);
            }

            disposable.Received(1).Dispose();
        }

        [Test]
        public async Task Run_Dispose()
        {
            var (service, disposable) = SubstituteExtensions.For<IFixtureSetUpService, IDisposable>();

            await CreateAndDisposeFrameworkScope(service).ConfigureAwait(false);

            disposable.Received().Dispose();
        }

        [Test]
        public async Task Run_DisposeAsync()
        {
            var (service, disposable) = SubstituteExtensions.For<IFixtureSetUpService, IAsyncDisposable>();

            await CreateAndDisposeFrameworkScope(service).ConfigureAwait(false);

            await disposable.Received().DisposeAsync().ConfigureAwait(false);
        }

        [Test]
        public async Task Run_Only_DisposeAsync_If_Both_Interfaces_Are_Implemented()
        {
            var (service, asyncDisposable, disposable) = SubstituteExtensions.For<IFixtureSetUpService, IAsyncDisposable, IDisposable>();

            await CreateAndDisposeFrameworkScope(service).ConfigureAwait(false);

            await asyncDisposable.Received().DisposeAsync().ConfigureAwait(false);
            disposable.DidNotReceive().Dispose();
        }

        private static async Task CreateAndDisposeFrameworkScope(IFixtureSetUpService service)
        {
            var framework = CreateFramework(service);

            await using (framework.ConfigureAwait(false))
            {
                var currentTest = TestExecutionContext.CurrentContext.CurrentTest;
                await framework.RunSetUp(currentTest).ConfigureAwait(false);

                await framework.RunTearDown(currentTest).ConfigureAwait(false);
            }
        }

        private static FixtureServiceProviderFramework CreateFramework(IFixtureSetUpService service) =>
            new(serviceCollection => serviceCollection.AddScoped(_ => service));
    }
}
