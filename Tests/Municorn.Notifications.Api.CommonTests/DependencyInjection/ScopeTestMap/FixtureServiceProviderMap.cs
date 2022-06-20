﻿using System;
using System.Runtime.CompilerServices;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.ScopeTestMap
{
    internal class FixtureServiceProviderMap
    {
        private readonly ConditionalWeakTable<object, IServiceProvider> serviceScopes = new();

        internal void AddScope(object fixture, IServiceProvider serviceScope) => this.serviceScopes.Add(fixture, serviceScope);

        internal IServiceProvider GetScope(object fixture) =>
            this.serviceScopes.TryGetValue(fixture, out var serviceScope)
                ? serviceScope
                : throw CreateNotFoundException(fixture);

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