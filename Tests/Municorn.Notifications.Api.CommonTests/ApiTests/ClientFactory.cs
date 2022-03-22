using System.Threading.Tasks;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.Tests.ApiTests
{
    [PrimaryConstructor]
    internal partial class ClientFactory
    {
        private readonly IClientTopologyFactory topologyFactory;
        private readonly ILog log;

        internal async Task<INotificationsClient> Create()
        {
            var topology = await this.topologyFactory.GetTopology().ConfigureAwait(false);
            return new NotificationsClient(new(topology, this.log));
        }
    }
}
