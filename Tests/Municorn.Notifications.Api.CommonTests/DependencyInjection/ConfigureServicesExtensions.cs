using System;
using System.Runtime.CompilerServices;

namespace Municorn.Notifications.Api.Tests.DependencyInjection
{
    internal static class ConfigureServicesExtensions
    {
        private static readonly ConditionalWeakTable<IConfigureServices, TestCaseServiceResolver> Storage = new();

        internal static void SaveServiceResolver(this IConfigureServices configureServices, TestCaseServiceResolver serviceResolver)
        {
            Storage.Add(configureServices, serviceResolver);
        }

        internal static TService ResolveService<TService>(this IConfigureServices configureServices)
            where TService : notnull
        {
            return Storage
                .GetValue(configureServices, _ => throw new InvalidOperationException($"Fixture {configureServices} doesn't contain scope scope service resolver"))
                .ResolveService<TService>();
        }
    }
}
