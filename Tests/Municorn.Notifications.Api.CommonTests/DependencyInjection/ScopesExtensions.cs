using System;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.Tests.DependencyInjection
{
    internal static class ScopesExtensions
    {
        private static readonly ConditionalWeakTable<ITest, IServiceScope> ServiceScopes = new();

        internal static void SaveScope(this ITest test, IServiceScope scope)
        {
            ServiceScopes.Add(test, scope);
        }

        internal static IServiceScope GetScope(this ITest test)
        {
            return ServiceScopes.TryGetValue(test, out var serviceScope)
                ? serviceScope
                : throw new InvalidOperationException($"Service scope is not found for test {test}");
        }

        internal static void RemoveScope(this ITest test)
        {
            ServiceScopes.Remove(test);
        }

        internal static TService ResolveService<TService>(this IConfigureServices configureServices)
            where TService : notnull
        {
            _ = configureServices;
            return TestExecutionContext.CurrentContext.CurrentTest.GetScope()
                .ServiceProvider
                .GetRequiredService<TService>();
        }
    }
}
