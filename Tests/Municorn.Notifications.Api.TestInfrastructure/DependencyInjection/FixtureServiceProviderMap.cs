using System;
using System.Runtime.CompilerServices;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection
{
    internal class FixtureServiceProviderMap
    {
        private readonly ConditionalWeakTable<object, IServiceProvider> serviceScopes = new();

        internal IServiceProvider Get(object fixture) =>
            this.serviceScopes.TryGetValue(fixture, out var serviceScope)
                ? serviceScope
                : throw CreateNotFoundException(fixture);

        internal void Add(object fixture, IServiceProvider serviceScope) => this.serviceScopes.Add(fixture, serviceScope);

        internal void Remove(object fixture)
        {
            if (!this.serviceScopes.Remove(fixture))
            {
                throw CreateNotFoundException(fixture);
            }
        }

        private static InvalidOperationException CreateNotFoundException(object fixture) => new($"Service provider is not found for fixture {fixture}");
    }
}
