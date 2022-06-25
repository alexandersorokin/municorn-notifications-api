using System.Collections.Generic;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework
{
    [PrimaryConstructor]
    internal partial class FixtureOneTimeSetUpRunner
    {
        private readonly IEnumerable<IFixtureOneTimeSetUpService> services;

        public void Run()
        {
            foreach (var service in this.services)
            {
                service.Run();
            }
        }
    }
}
