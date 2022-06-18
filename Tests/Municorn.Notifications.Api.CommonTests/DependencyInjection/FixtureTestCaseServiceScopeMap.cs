using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.Tests.DependencyInjection
{
    internal class FixtureTestCaseServiceScopeMap
    {
        private readonly ConcurrentDictionary<ITest, AsyncServiceScope> serviceScopes = new();

        internal void AddScope(ITest test, AsyncServiceScope scope)
        {
            if (!this.serviceScopes.TryAdd(test, scope))
            {
                throw new InvalidOperationException($"Service scope is already added for test {test}");
            }
        }

        internal TService ResolveService<TService>(ITest test)
            where TService : notnull
        {
            return this.serviceScopes.TryGetValue(test, out var serviceScope)
                ? serviceScope.ServiceProvider.GetRequiredService<TService>()
                : throw CreateNotFoundException(test);
        }

        internal void DisposeScope(ITest test)
        {
            if (this.serviceScopes.TryRemove(test, out var value))
            {
                value.DisposeSynchronously();
            }
            else
            {
                throw CreateNotFoundException(test);
            }
        }

        private static InvalidOperationException CreateNotFoundException(ITest test)
        {
            return new($"Service scope is not found for test {test}");
        }
    }
}
