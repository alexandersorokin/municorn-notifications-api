using System;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Communication.AsyncLocal
{
    public class AsyncLocalServiceProvider : IServiceProvider
    {
        private readonly IFixtureProvider fixtureProvider;

        internal AsyncLocalServiceProvider(IFixtureProvider fixtureProvider) => this.fixtureProvider = fixtureProvider;

        public object? GetService(Type serviceType) =>
            TestExecutionContext.CurrentContext.CurrentTest
                .GetServiceProvider(this.fixtureProvider.Fixture)
                .GetService(serviceType);
    }
}
