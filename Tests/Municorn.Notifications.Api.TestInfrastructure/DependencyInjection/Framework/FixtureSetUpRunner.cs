using System.Collections.Generic;
using System.Threading.Tasks;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework
{
    [PrimaryConstructor]
    internal sealed partial class FixtureSetUpRunner
    {
        private readonly IEnumerable<IFixtureSetUpService> services;
        private readonly IEnumerable<IFixtureSetUpAsyncService> asyncServices;

        public async Task RunAsync()
        {
            foreach (var service in this.services)
            {
                service.Run();
            }

            foreach (var service in this.asyncServices)
            {
                await service.RunAsync().ConfigureAwait(false);
            }
        }
    }
}
