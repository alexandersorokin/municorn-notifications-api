using System.Collections.Generic;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.TestActionManagers
{
    [PrimaryConstructor]
    internal partial class FixtureSetUpRunner
    {
        private readonly IEnumerable<IFixtureSetUp> services;

        public void Run()
        {
            foreach (var service in this.services)
            {
                service.Run();
            }
        }
    }
}
