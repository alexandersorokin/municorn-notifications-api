using System.Collections.Generic;
using System.Threading.Tasks;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework
{
    [PrimaryConstructor]
    internal partial class FixtureOneTimeSetUpRunner
    {
        private readonly IEnumerable<IFixtureOneTimeSetUpService> services;
        private readonly IEnumerable<IFixtureOneTimeSetUpAsyncService> asyncServices;

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
