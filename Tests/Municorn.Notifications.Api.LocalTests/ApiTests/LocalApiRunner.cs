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
        private readonly SetupFixtureProvider<GlobalLifetimeManager> lifecycleManagerProvider;
        private readonly SetupFixtureProvider<GlobalLog> boundLogProvider;

        public async Task<LocalApi> Start()
        {
            Uri uri = new($"http://localhost:{GetFreePort()}", UriKind.Absolute);
            var log = this.boundLogProvider.GetSetupFixture().BoundLog;
            HostSettings settings = new(
                uri,
                loggerBuilder => loggerBuilder.AddVostok(log));
            var host = NotificationApiApplication.CreateHost(settings);

#pragma warning disable CA2000 // Dispose objects before losing scope
            HostDisposeWrapper hostDisposeWrapper = new(host);
#pragma warning restore CA2000 // Dispose objects before losing scope
            this.lifecycleManagerProvider.GetSetupFixture().RegisterForDispose(hostDisposeWrapper);

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
