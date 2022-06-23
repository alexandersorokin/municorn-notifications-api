using System;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Scopes;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Communication
{
    internal sealed class ScopeSaver : IFixtureSetUp, IDisposable
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ITestFixtureProvider testFixtureProvider;
        private readonly FixtureServiceProviderMap map;

        public ScopeSaver(IServiceProvider serviceProvider, ITestFixtureProvider testFixtureProvider, TestAccessor testAccessor)
        {
            this.serviceProvider = serviceProvider;
            this.testFixtureProvider = testFixtureProvider;
            this.map = testAccessor.Test.GetFixtureServiceProviderMap();
        }

        public void Run() => this.map.AddScope(this.testFixtureProvider.Fixture, this.serviceProvider);

        public void Dispose() => this.map.RemoveScope(this.testFixtureProvider.Fixture);
    }
}
