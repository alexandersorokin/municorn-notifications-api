using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework
{
    [PrimaryConstructor]
    internal sealed partial class ScopesManager
    {
        private readonly ConcurrentDictionary<ITest, IAsyncDisposable> scopes = new();
        private readonly IServiceProvider serviceProvider;

        internal IServiceProvider CreateScope(ITest test)
        {
            var serviceScope = this.serviceProvider.CreateAsyncScope();
            return this.scopes.TryAdd(test, serviceScope)
                ? serviceScope.ServiceProvider
                : throw new InvalidOperationException($"Failed to store service scope for {test.FullName}");
        }

        internal async Task DisposeScope(ITest test)
        {
            if (!this.scopes.TryRemove(test, out var scope))
            {
                throw new InvalidOperationException($"Failed to receive stored scope for {test.FullName}");
            }

            await scope.DisposeAsync().ConfigureAwait(false);
        }
    }
}
