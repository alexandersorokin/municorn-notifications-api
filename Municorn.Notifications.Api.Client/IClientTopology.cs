using Vostok.Clusterclient.Core;

namespace Municorn.Notifications.Api
{
    public interface IClientTopology
    {
        void Setup(IClusterClientConfiguration configuration);
    }
}