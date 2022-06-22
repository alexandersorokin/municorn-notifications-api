using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.ScopeMethodInject;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.TestActionManagers
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
            if (!this.scopes.TryAdd(test, new(serviceScope, originalMethodInfo)))
            {
                throw new InvalidOperationException($"Failed to save original MethodInfo for {test.FullName}");
            }

            var scopeServiceProvider = serviceScope.ServiceProvider;
            var testAccessor = scopeServiceProvider.GetRequiredService<TestAccessor>();
            testAccessor.Test = test;
            testAccessor.ServiceProvider = scopeServiceProvider;

            testMethod.Method = new UseContainerMethodInfo(originalMethodInfo, scopeServiceProvider, this.fixtureProvider.Fixture);
            scopeServiceProvider.GetRequiredService<FixtureSetUpRunner>().Run();
        }

        internal void AfterTestCase(ITest test)
        {
            if (!this.scopes.TryRemove(test, out var testData))
            {
                throw new InvalidOperationException($"Failed to get saved TestData for {test.FullName}");
            }

            ((TestMethod)test).Method = testData.OriginalMethodInfo;
            testData.Scope.DisposeSynchronously();
        }

        private record TestData(AsyncServiceScope Scope, IMethodInfo OriginalMethodInfo);
    }
}
