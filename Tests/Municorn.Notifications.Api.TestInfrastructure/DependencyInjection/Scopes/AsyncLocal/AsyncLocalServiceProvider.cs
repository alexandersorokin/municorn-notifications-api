using System;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Communication;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Scopes.AsyncLocal
{
    public class AsyncLocalServiceProvider : IServiceProvider
    {
        private readonly ITestFixtureProvider testFixtureProvider;

        internal AsyncLocalServiceProvider(ITestFixtureProvider testFixtureProvider) => this.testFixtureProvider = testFixtureProvider;

        public object? GetService(Type serviceType) =>
            TestExecutionContext.CurrentContext.CurrentTest
                .GetServiceProvider(this.testFixtureProvider.Fixture)
                .GetService(serviceType);
    }
}
