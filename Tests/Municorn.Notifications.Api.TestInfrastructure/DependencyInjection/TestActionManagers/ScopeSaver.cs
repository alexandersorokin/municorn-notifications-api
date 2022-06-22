using System;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.TestActionManagers
{
    [PrimaryConstructor]
    internal sealed partial class ScopeSaver : IFixtureSetUp, IDisposable
    {
        private readonly IFixtureProvider fixtureProvider;
        private readonly TestAccessor testAccessor;

        public void Run()
        {
            var map = this.testAccessor.Test.GetFixtureServiceProviderMap();
            map.AddScope(this.fixtureProvider.Fixture, this.testAccessor.ServiceProvider);
        }

        public void Dispose()
        {
            this.testAccessor.Test.GetFixtureServiceProviderMap().RemoveScope(this.fixtureProvider.Fixture);
        }
    }
}
