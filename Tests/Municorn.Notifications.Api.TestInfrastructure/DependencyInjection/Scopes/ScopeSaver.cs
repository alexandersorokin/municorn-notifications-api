using System;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Scopes
{
    internal sealed class ScopeSaver : IFixtureSetUp, IDisposable
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IFixtureProvider fixtureProvider;
        private readonly FixtureServiceProviderMap map;

        public ScopeSaver(IServiceProvider serviceProvider, IFixtureProvider fixtureProvider, TestAccessor testAccessor)
        {
            this.serviceProvider = serviceProvider;
            this.fixtureProvider = fixtureProvider;
            this.map = testAccessor.Test.GetFixtureServiceProviderMap();
        }

        public void Run() => this.map.AddScope(this.fixtureProvider.Fixture, this.serviceProvider);

        public void Dispose() => this.map.RemoveScope(this.fixtureProvider.Fixture);
    }
}
