using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework
{
    [PrimaryConstructor]
    internal partial class TestCaseScopesManager
    {
        private readonly ConcurrentDictionary<ITest, IAsyncDisposable> scopes = new();
        private readonly IServiceProvider fixtureServiceProvider;

        internal async Task BeforeTestCase(ITest test)
        {
            var serviceScope = this.fixtureServiceProvider.CreateAsyncScope();
            if (!this.scopes.TryAdd(test, serviceScope))
            {
                throw new InvalidOperationException($"Failed to store service scope for {test.FullName}");
            }

            var scopeServiceProvider = serviceScope.ServiceProvider;
            scopeServiceProvider.GetRequiredService<TestAccessor>().Test = test;

            await scopeServiceProvider.GetRequiredService<FixtureSetUpRunner>().RunAsync().ConfigureAwait(false);
        }

        internal async Task AfterTestCase(ITest test)
        {
            if (!this.scopes.TryRemove(test, out var scope))
            {
                throw new InvalidOperationException($"Failed to receive stored scope for {test.FullName}");
            }

            await scope.DisposeAsync().ConfigureAwait(false);
        }
    }
}
