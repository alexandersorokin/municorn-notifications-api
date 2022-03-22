using System.Threading.Tasks;

namespace Municorn.Notifications.Api.Tests.ApiTests
{
    [PrimaryConstructor]
    internal partial class LocalApiTopologyFactory : IClientTopologyFactory
    {
        private readonly LocalApiRunnerCache runnerCache;

        public async Task<IClientTopology> GetTopology()
        {
            var api = await this.runnerCache.Start().ConfigureAwait(false);
            return new DirectClientTopology(api.Uri);
        }
    }
}
