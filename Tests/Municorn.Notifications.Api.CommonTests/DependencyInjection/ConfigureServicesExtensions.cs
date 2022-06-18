using System;
using System.Runtime.CompilerServices;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.Tests.DependencyInjection
{
    internal static class ConfigureServicesExtensions
    {
        private static readonly ConditionalWeakTable<IConfigureServices, FixtureTestCaseServiceScopeMap> Storage = new();

        internal static void SaveMap(this IConfigureServices configureServices, FixtureTestCaseServiceScopeMap map)
        {
            Storage.Add(configureServices, map);
        }

        internal static TService ResolveService<TService>(this IConfigureServices configureServices)
            where TService : notnull
        {
            return Storage
                .GetValue(configureServices, _ => throw new InvalidOperationException($"Fixture {configureServices} doesn't contain scopes"))
                .ResolveService<TService>(TestExecutionContext.CurrentContext.CurrentTest);
        }
    }
}
