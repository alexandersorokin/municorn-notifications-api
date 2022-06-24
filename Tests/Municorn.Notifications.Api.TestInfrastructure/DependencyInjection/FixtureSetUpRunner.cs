using System.Collections.Generic;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection
{
    [PrimaryConstructor]
    internal partial class FixtureSetUpRunner
    {
        private readonly IEnumerable<IFixtureSetUpService> services;

        public void Run()
        {
            foreach (var service in this.services)
            {
                service.Run();
            }
        }
    }
}
