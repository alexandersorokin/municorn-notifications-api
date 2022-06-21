using System.Collections.Generic;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AutoMethods
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
