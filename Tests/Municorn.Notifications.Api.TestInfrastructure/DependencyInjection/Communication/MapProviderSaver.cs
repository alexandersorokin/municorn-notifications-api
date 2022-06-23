using System;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Communication
{
    internal sealed class MapProviderSaver : IFixtureOneTimeSetUp, IDisposable
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ITestFixtureProvider testFixtureProvider;
        private readonly FixtureServiceProviderMap map;

        public MapProviderSaver(IServiceProvider serviceProvider, ITestFixtureProvider testFixtureProvider, ITest test)
        {
            this.serviceProvider = serviceProvider;
            this.testFixtureProvider = testFixtureProvider;
            this.map = test.GetFixtureServiceProviderMap();
        }

        public void Run() => this.map.AddScope(this.testFixtureProvider.Fixture, this.serviceProvider);

        public void Dispose() => this.map.RemoveScope(this.testFixtureProvider.Fixture);
    }
}
