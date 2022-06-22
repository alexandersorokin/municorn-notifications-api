using System;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Scopes
{
    internal sealed class ScopeSaver : IFixtureSetUp, IDisposable
    {
        private readonly IFixtureProvider fixtureProvider;
        private readonly TestAccessor testAccessor;
        private readonly FixtureServiceProviderMap map;

        public ScopeSaver(IFixtureProvider fixtureProvider, TestAccessor testAccessor)
        {
            this.fixtureProvider = fixtureProvider;
            this.testAccessor = testAccessor;
            this.map = this.testAccessor.Test.GetFixtureServiceProviderMap();
        }

        public void Run() => this.map.AddScope(this.fixtureProvider.Fixture, this.testAccessor.ServiceProvider);

        public void Dispose() => this.map.RemoveScope(this.fixtureProvider.Fixture);
    }
}
