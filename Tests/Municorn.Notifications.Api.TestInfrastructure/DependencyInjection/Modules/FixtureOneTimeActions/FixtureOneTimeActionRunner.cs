using System;
using System.Threading.Tasks;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FixtureOneTimeActions
{
    [PrimaryConstructor]
    internal partial class FixtureOneTimeActionRunner : IFixtureOneTimeSetUpService, IAsyncDisposable
    {
        private readonly IFixtureProvider fixtureProvider;

        public void Run()
        {
            var fixture = this.fixtureProvider.Fixture;
            if (fixture is IOneTimeSetUpAction oneTimeSetUp)
            {
                oneTimeSetUp.OneTimeSetUp();
            }

            if (fixture is IOneTimeSetUpAsyncAction oneTimeSetUpAsync)
            {
                oneTimeSetUpAsync.OneTimeSetUpAsync().GetAwaiter().GetResult();
            }
        }

        public async ValueTask DisposeAsync()
        {
            var fixture = this.fixtureProvider.Fixture;
            if (fixture is IOneTimeTearDownAsyncAction oneTimeTearDownAsync)
            {
                await oneTimeTearDownAsync.IOneTimeTearDownAsync().ConfigureAwait(false);
            }

            if (fixture is IOneTimeTearDownAction oneTimeTearDown)
            {
                oneTimeTearDown.OneTimeTearDown();
            }
        }
    }
}
