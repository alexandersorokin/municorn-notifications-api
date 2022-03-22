using System;
using Vostok.Clusterclient.Core;
using Vostok.Clusterclient.Core.Strategies;
using Vostok.Clusterclient.Core.Topology;

namespace Municorn.Notifications.Api
{
    [PrimaryConstructor]
    public partial class DirectClientTopology : IClientTopology
    {
        private readonly Uri serviceUri;

        public void Setup(IClusterClientConfiguration configuration)
        {
            configuration.ClusterProvider = new FixedClusterProvider(this.serviceUri);
            configuration.DefaultRequestStrategy = Strategy.SingleReplica;
        }
    }
}