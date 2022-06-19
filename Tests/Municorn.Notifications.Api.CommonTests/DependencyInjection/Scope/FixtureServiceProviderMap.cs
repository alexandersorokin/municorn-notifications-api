﻿using System;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.Scope
{
    internal class FixtureServiceProviderMap
    {
        private readonly ConditionalWeakTable<object, ReferenceTypeConverter> serviceScopes = new();

        internal void AddScope(object fixture, AsyncServiceScope serviceScope) => this.serviceScopes.Add(fixture, new(serviceScope));

        internal AsyncServiceScope GetScope(object fixture) =>
            this.serviceScopes.TryGetValue(fixture, out var serviceScope)
                ? serviceScope.Scope
                : throw CreateNotFoundException(fixture);

        internal void RemoveScope(object fixture)
        {
            if (!this.serviceScopes.Remove(fixture))
            {
                throw CreateNotFoundException(fixture);
            }
        }

        private static InvalidOperationException CreateNotFoundException(object fixture) => new($"Service scope is not found for fixture {fixture}");

        private record ReferenceTypeConverter(AsyncServiceScope Scope);
    }
}