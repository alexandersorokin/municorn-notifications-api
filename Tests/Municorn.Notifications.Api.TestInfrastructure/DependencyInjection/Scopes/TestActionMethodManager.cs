using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Scopes
{
    [PrimaryConstructor]
    internal partial class TestActionMethodManager
    {
        private readonly ConcurrentDictionary<ITest, IAsyncDisposable> scopes = new();
        private readonly IServiceProvider fixtureServiceProvider;

        internal void BeforeTestCase(ITest test)
        {
            var serviceScope = this.fixtureServiceProvider.CreateAsyncScope();
            if (!this.scopes.TryAdd(test, serviceScope))
            {
                throw new InvalidOperationException($"Failed to save original MethodInfo for {test.FullName}");
            }

            var serviceProvider = serviceScope.ServiceProvider;
            serviceProvider.GetRequiredService<TestAccessor>().Test = test;
            serviceProvider.GetRequiredService<FixtureSetUpRunner>().Run();
        }

        internal void AfterTestCase(ITest test)
        {
            if (!this.scopes.TryRemove(test, out var scope))
            {
                throw new InvalidOperationException($"Failed to get saved scope for {test.FullName}");
            }

            scope.DisposeAsync().AsTask().GetAwaiter().GetResult();
        }
    }
}
