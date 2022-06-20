using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.Tests.DependencyInjection.ScopeMethodInject;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.Tests.DependencyInjection
{
    [PrimaryConstructor]
    internal partial class TestActionMethodManager
    {
        private readonly ConcurrentDictionary<ITest, TestData> scopes = new();

        private readonly IFixtureProvider fixtureProvider;

        internal void BeforeTestCase(ServiceProvider serviceProvider, ITest test)
        {
            var serviceScope = serviceProvider.CreateAsyncScope();

            var testMethod = (TestMethod)test;
            var originalMethodInfo = testMethod.Method;
            var map = test.GetFixtureServiceProviderMap();
            if (!this.scopes.TryAdd(test, new(serviceScope, originalMethodInfo, map)))
            {
                throw new InvalidOperationException($"Failed to save original MethodInfo for {test.FullName}");
            }

            map.AddScope(this.fixtureProvider.Fixture, serviceScope.ServiceProvider);
            testMethod.Method = new UseContainerMethodInfo(originalMethodInfo, serviceScope.ServiceProvider, this.fixtureProvider.Fixture);
        }

        internal void AfterTestCase(ServiceProvider serviceProvider, ITest test)
        {
            if (!this.scopes.TryRemove(test, out var testData))
            {
                throw new InvalidOperationException($"Failed to get saved TestData for {test.FullName}");
            }

            ((TestMethod)test).Method = testData.OriginalMethodInfo;
            testData.Map.RemoveScope(this.fixtureProvider.Fixture);
            testData.Scope.DisposeSynchronously();
        }

        private record TestData(AsyncServiceScope Scope, IMethodInfo OriginalMethodInfo, FixtureServiceProviderMap Map);
    }
}
