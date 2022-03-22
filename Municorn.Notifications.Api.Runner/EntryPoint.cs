using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Municorn.Notifications.Api.Infrastructure;

namespace Municorn.Notifications.Api.Runner
{
    internal static class EntryPoint
    {
        internal static async Task Main()
        {
            HostSettings settings = new(
                new("http://localhost:43920", UriKind.Absolute),
                loggerBuilder => loggerBuilder.AddConsole());
            var host = NotificationApiApplication.CreateHost(settings);
            using (host)
            {
                await host.RunAsync().ConfigureAwait(false);
            }
        }
    }
}
