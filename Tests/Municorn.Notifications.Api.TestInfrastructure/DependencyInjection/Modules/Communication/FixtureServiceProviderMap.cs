using System;
using System.Runtime.CompilerServices;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Communication
{
    internal class FixtureServiceProviderMap
    {
        private readonly ConditionalWeakTable<object, IServiceProvider> serviceScopes = new();

        internal IServiceProvider GetScope(object fixture) =>
            this.serviceScopes.TryGetValue(fixture, out var serviceScope)
                ? serviceScope
                : throw CreateNotFoundException(fixture);

        internal void AddScope(object fixture, IServiceProvider serviceScope) => this.serviceScopes.Add(fixture, serviceScope);

        internal void RemoveScope(object fixture)
        {
            if (!this.serviceScopes.Remove(fixture))
            {
                throw CreateNotFoundException(fixture);
            }
        }

        private static InvalidOperationException CreateNotFoundException(object fixture) => new($"Service scope is not found for fixture {fixture}");
    }
}
