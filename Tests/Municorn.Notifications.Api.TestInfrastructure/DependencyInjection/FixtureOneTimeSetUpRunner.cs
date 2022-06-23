using System.Collections.Generic;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection
{
    [PrimaryConstructor]
    internal partial class FixtureOneTimeSetUpRunner
    {
        private readonly IEnumerable<IFixtureOneTimeSetUp> services;

        public void Run()
        {
            foreach (var service in this.services)
            {
                service.Run();
            }
        }
    }
}
