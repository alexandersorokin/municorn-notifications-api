using System;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AutoMethods
{
    internal sealed class ProviderSaver : IFixtureOneTimeSetUp, IDisposable
    {
        private readonly IFixtureProvider fixtureProvider;
        private readonly ServiceProviderAccessor serviceProviderAccessor;
        private readonly FixtureServiceProviderMap map;

        public ProviderSaver(ITest test, IFixtureProvider fixtureProvider, ServiceProviderAccessor serviceProviderAccessor)
        {
            this.fixtureProvider = fixtureProvider;
            this.serviceProviderAccessor = serviceProviderAccessor;
            this.map = test.GetFixtureServiceProviderMap();
        }

        public void Run() => this.map.AddScope(this.fixtureProvider.Fixture, this.serviceProviderAccessor.ServiceProvider);

        public void Dispose() => this.map.RemoveScope(this.fixtureProvider.Fixture);
    }
}
