using System;
using System.Threading.Tasks;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FixtureOneTimeActions
{
    [PrimaryConstructor]
    internal partial class FixtureOneTimeActionRunner : IFixtureOneTimeSetUp, IAsyncDisposable
    {
        private readonly IFixtureProvider fixtureProvider;

        public void Run()
        {
            var fixture = this.fixtureProvider.Fixture;
            if (fixture is IOneTimeSetUp oneTimeSetUp)
            {
                oneTimeSetUp.OneTimeSetUp();
            }

            if (fixture is IOneTimeSetUpAsync oneTimeSetUpAsync)
            {
                oneTimeSetUpAsync.OneTimeSetUpAsync().GetAwaiter().GetResult();
            }
        }

        public async ValueTask DisposeAsync()
        {
            var fixture = this.fixtureProvider.Fixture;
            if (fixture is IOneTimeTearDownAsync oneTimeTearDownAsync)
            {
                await oneTimeTearDownAsync.IOneTimeTearDownAsync().ConfigureAwait(false);
            }

            if (fixture is IOneTimeTearDown oneTimeTearDown)
            {
                oneTimeTearDown.OneTimeTearDown();
            }
        }
    }
}
