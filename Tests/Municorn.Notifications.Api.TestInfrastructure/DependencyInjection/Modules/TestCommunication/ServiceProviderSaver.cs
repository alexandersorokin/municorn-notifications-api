using System;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.TestCommunication
{
    internal sealed class ServiceProviderSaver : IFixtureOneTimeSetUpService, IDisposable
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IFixtureProvider fixtureProvider;
        private readonly FixtureServiceProviderMap map;

        public ServiceProviderSaver(IServiceProvider serviceProvider, IFixtureProvider fixtureProvider, ITest test)
        {
            this.serviceProvider = serviceProvider;
            this.fixtureProvider = fixtureProvider;
            this.map = test.GetFixtureServiceProviderMap();
        }

        public void Run() => this.map.Add(this.fixtureProvider.Fixture, this.serviceProvider);

        public void Dispose() => this.map.Remove(this.fixtureProvider.Fixture);
    }
}
