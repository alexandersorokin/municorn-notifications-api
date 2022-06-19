using System.Threading.Tasks;

namespace Municorn.Notifications.Api.Tests.ApiTests
{
    [PrimaryConstructor]
    internal partial class LocalApiRunnerCache
    {
        private readonly GlobalCache globalCache;
        private readonly LocalApiRunner runner;

        public async Task<LocalApi> Start() =>
            await this.globalCache
                .GetOrCreate(() => this.runner.Start())
                .ConfigureAwait(false);
    }
}
