using System;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.TestCommunication.AsyncLocal
{
    [PrimaryConstructor]
    internal partial class AsyncLocalServiceProvider : IAsyncLocalServiceProvider
    {
        private readonly IFixtureProvider fixtureProvider;

        public object? GetService(Type serviceType) =>
            TestExecutionContext.CurrentContext.CurrentTest
                .GetServiceProvider(this.fixtureProvider.Fixture)
                .GetService(serviceType);
    }
}
