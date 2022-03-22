using System.Threading.Tasks;

namespace Municorn.Notifications.Api.Tests.ApiTests
{
    [PrimaryConstructor]
    internal partial class LocalApiRunnerCache
    {
        private readonly SetupFixtureProvider<GlobalCache> cacheProvider;
        private readonly LocalApiRunner runner;

        public async Task<LocalApi> Start()
        {
            return await this.cacheProvider
                .GetSetupFixture()
                .GetOrCreate(() => this.runner.Start())
                .ConfigureAwait(false);
        }
    }
}
