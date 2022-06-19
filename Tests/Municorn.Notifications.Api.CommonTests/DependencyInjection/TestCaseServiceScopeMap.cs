﻿using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.Tests.DependencyInjection
{
    internal class TestCaseServiceScopeMap
    {
        private readonly ConcurrentDictionary<ITest, AsyncServiceScope> serviceScopes = new();

        internal void AddScope(ITest test, AsyncServiceScope scope)
        {
            if (!this.serviceScopes.TryAdd(test, scope))
            {
                throw new InvalidOperationException($"Service scope is already added for test {test.FullName}");
            }
        }

        internal AsyncServiceScope GetScope(ITest test)
        {
            return this.serviceScopes.TryGetValue(test, out var serviceScope)
                ? serviceScope
                : throw CreateNotFoundException(test);
        }

        internal AsyncServiceScope RemoveScope(ITest test)
        {
            return this.serviceScopes.TryRemove(test, out var serviceScope)
                ? serviceScope
                : throw CreateNotFoundException(test);
        }

        private static InvalidOperationException CreateNotFoundException(ITest test)
        {
            return new($"Service scope is not found for test {test.FullName}");
        }
    }
}
