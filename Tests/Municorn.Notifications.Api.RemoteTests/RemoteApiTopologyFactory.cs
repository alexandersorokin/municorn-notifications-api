using System;
using System.Threading.Tasks;

namespace Municorn.Notifications.Api.Tests.ApiTests
{
    internal class RemoteApiTopologyFactory : IClientTopologyFactory
    {
        public Task<IClientTopology> GetTopology()
        {
            DirectClientTopology directClientTopology = new(new("http://localhost:43920", UriKind.Absolute));
            return Task.FromResult<IClientTopology>(directClientTopology);
        }
    }
}