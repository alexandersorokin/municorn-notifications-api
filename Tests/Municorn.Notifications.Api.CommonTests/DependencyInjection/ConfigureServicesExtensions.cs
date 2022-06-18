using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.Tests.DependencyInjection
{
    internal static class ConfigureServicesExtensions
    {
        private static readonly ConditionalWeakTable<IConfigureServices, FixtureTestCaseServiceScopeMap> Storage = new();

        internal static void AddScope(this IConfigureServices configureServices, ITest test, AsyncServiceScope scope)
        {
            Storage.GetOrCreateValue(configureServices).AddScope(test, scope);
        }

        internal static void DisposeScope(this IConfigureServices configureServices, ITest test)
        {
            Storage.GetOrCreateValue(configureServices).DisposeScope(test);
        }

        internal static TService ResolveService<TService>(IConfigureServices configureServices, ITest currentTest)
            where TService : notnull
        {
            return Storage
                .GetOrCreateValue(configureServices)
                .GetResolveService<TService>(currentTest);
        }

        internal static TService ResolveService<TService>(this IConfigureServices configureServices)
            where TService : notnull
        {
            return ResolveService<TService>(configureServices, TestExecutionContext.CurrentContext.CurrentTest);
        }
    }
}
