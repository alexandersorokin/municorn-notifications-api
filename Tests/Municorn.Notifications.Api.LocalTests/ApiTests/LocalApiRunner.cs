using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Municorn.Notifications.Api.Infrastructure;
using Vostok.Logging.Microsoft;

namespace Municorn.Notifications.Api.Tests.ApiTests
{
    [PrimaryConstructor]
    internal partial class LocalApiRunner
    {
        private readonly GlobalLifetimeManager lifecycleManager;
        private readonly GlobalLog globalLog;

        public async Task<LocalApi> Start()
        {
            Uri uri = new($"http://localhost:{GetFreePort()}", UriKind.Absolute);
            HostSettings settings = new(
                uri,
                loggerBuilder => loggerBuilder.AddVostok(this.globalLog.BoundLog));
            var host = NotificationApiApplication.CreateHost(settings);

#pragma warning disable CA2000 // Dispose objects before losing scope
            HostDisposeWrapper hostDisposeWrapper = new(host);
#pragma warning restore CA2000 // Dispose objects before losing scope
            this.lifecycleManager.RegisterForDispose(hostDisposeWrapper);

            await host.StartAsync().ConfigureAwait(false);

            return new(uri);
        }

        private static int GetFreePort()
        {
            TcpListener listener = new(IPAddress.Loopback, 0);
            try
            {
                listener.Start();
                return ((IPEndPoint)listener.LocalEndpoint).Port;
            }
            finally
            {
                listener.Stop();
            }
        }
    }
}
