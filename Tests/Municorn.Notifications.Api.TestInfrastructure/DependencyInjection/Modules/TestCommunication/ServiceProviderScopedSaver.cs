using System;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.TestCommunication
{
    internal sealed class ServiceProviderScopedSaver : IFixtureSetUpService, IDisposable
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IFixtureProvider fixtureProvider;
        private readonly FixtureServiceProviderMap map;

        public ServiceProviderScopedSaver(IServiceProvider serviceProvider, IFixtureProvider fixtureProvider, TestAccessor testAccessor)
        {
            this.serviceProvider = serviceProvider;
            this.fixtureProvider = fixtureProvider;
            this.map = testAccessor.Test.GetFixtureServiceProviderMap();
        }

        public void Run() => this.map.Add(this.fixtureProvider.Fixture, this.serviceProvider);

        public void Dispose() => this.map.Remove(this.fixtureProvider.Fixture);
    }
}
