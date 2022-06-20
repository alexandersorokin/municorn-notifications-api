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

        internal void BeforeTestCase(ServiceProvider fixtureServiceProvider, ITest test)
        {
            var serviceScope = fixtureServiceProvider.CreateAsyncScope();

            var testMethod = (TestMethod)test;
            var originalMethodInfo = testMethod.Method;
            var map = test.GetFixtureServiceProviderMap();
            if (!this.scopes.TryAdd(test, new(serviceScope, originalMethodInfo, map)))
            {
                throw new InvalidOperationException($"Failed to save original MethodInfo for {test.FullName}");
            }

            var scopeServiceProvider = serviceScope.ServiceProvider;
            scopeServiceProvider.GetRequiredService<TestAccessor>().Test = test;
            testMethod.Method = new UseContainerMethodInfo(originalMethodInfo, scopeServiceProvider, this.fixtureProvider.Fixture);
            map.AddScope(this.fixtureProvider.Fixture, scopeServiceProvider);

            foreach (var fixtureSetUp in scopeServiceProvider.GetServices<IFixtureSetUp>())
            {
                fixtureSetUp.Run();
            }
        }

        internal void AfterTestCase(ITest test)
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
