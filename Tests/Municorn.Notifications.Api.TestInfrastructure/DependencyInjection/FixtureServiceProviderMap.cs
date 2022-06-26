using System;
using System.Runtime.CompilerServices;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection
{
    internal sealed class FixtureServiceProviderMap
    {
        private readonly ConditionalWeakTable<object, IServiceProvider> serviceScopes = new();

        internal IServiceProvider Get(object fixture) =>
            this.serviceScopes.TryGetValue(fixture, out var serviceScope)
                ? serviceScope
                : throw new InvalidOperationException($"Service provider is not found for fixture {fixture}");

        internal void Add(object fixture, IServiceProvider serviceScope) => this.serviceScopes.Add(fixture, serviceScope);

        internal void Remove(object fixture) => this.serviceScopes.Remove(fixture);
    }
}
