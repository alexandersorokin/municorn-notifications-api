using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api
{
    public record NotificationsClientOptions(
        IClientTopology Topology,
        ILog Log);
}