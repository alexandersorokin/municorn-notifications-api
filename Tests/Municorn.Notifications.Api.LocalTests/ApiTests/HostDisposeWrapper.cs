using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Municorn.Notifications.Api.Tests.ApiTests
{
    [PrimaryConstructor]
    internal sealed partial class HostDisposeWrapper : IAsyncDisposable
    {
        private readonly IHost host;

        public async ValueTask DisposeAsync()
        {
            await this.host.StopAsync().ConfigureAwait(false);
            this.host.Dispose();
        }
    }
}
