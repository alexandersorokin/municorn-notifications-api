using System;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

namespace Municorn.Notifications.Api.Tests.DependencyInjection
{
    internal class FixtureServiceProviderMap
    {
        private readonly ConditionalWeakTable<IConfigureServices, ReferenceTypeConverter> serviceScopes = new();

        internal void AddScope(IConfigureServices fixture, AsyncServiceScope serviceScope) => this.serviceScopes.Add(fixture, new(serviceScope));

        internal AsyncServiceScope GetScope(IConfigureServices fixture) =>
            this.serviceScopes.TryGetValue(fixture, out var serviceScope)
                ? serviceScope.Scope
                : throw CreateNotFoundException(fixture);

        internal void RemoveScope(IConfigureServices fixture)
        {
            if (!this.serviceScopes.Remove(fixture))
            {
                throw CreateNotFoundException(fixture);
            }
        }

        private static InvalidOperationException CreateNotFoundException(IConfigureServices fixture) => new($"Service scope is not found for fixture {fixture}");

        private record ReferenceTypeConverter(AsyncServiceScope Scope);
    }
}
