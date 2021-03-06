using System;
using System.Threading.Tasks;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FixtureOneTimeActions
{
    [PrimaryConstructor]
    internal sealed partial class FixtureOneTimeActionRunner : IFixtureOneTimeSetUpAsyncService, IAsyncDisposable
    {
        private readonly IFixtureProvider fixtureProvider;

        public async Task RunAsync()
        {
            var fixture = this.fixtureProvider.Fixture;
            if (fixture is IOneTimeSetUpAction oneTimeSetUp)
            {
                oneTimeSetUp.OneTimeSetUp();
            }

            if (fixture is IOneTimeSetUpAsyncAction oneTimeSetUpAsync)
            {
                await oneTimeSetUpAsync.OneTimeSetUpAsync().ConfigureAwait(false);
            }
        }

        public async ValueTask DisposeAsync()
        {
            var fixture = this.fixtureProvider.Fixture;
            if (fixture is IOneTimeTearDownAsyncAction oneTimeTearDownAsync)
            {
                await oneTimeTearDownAsync.OneTimeTearDownAsync().ConfigureAwait(false);
            }

            if (fixture is IOneTimeTearDownAction oneTimeTearDown)
            {
                oneTimeTearDown.OneTimeTearDown();
            }
        }
    }
}
