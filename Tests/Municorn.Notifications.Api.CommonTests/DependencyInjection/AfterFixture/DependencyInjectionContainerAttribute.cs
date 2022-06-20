using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.Tests.DependencyInjection.ScopeMethodInject;
using Municorn.Notifications.Api.Tests.DependencyInjection.ScopeTestMap;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.AfterFixture
{
    [AttributeUsage(AttributeTargets.Interface)]
    internal sealed class DependencyInjectionContainerAttribute : NUnitAttribute, ITestAction
    {
        private static readonly ServiceProviderOptions Options = new()
        {
            ValidateOnBuild = true,
            ValidateScopes = true,
        };

        private readonly ConcurrentDictionary<ITest, TestData> scopes = new();
        private ServiceProvider? serviceProvider;

        public ActionTargets Targets => ActionTargets.Suite | ActionTargets.Test;

        public void BeforeTest(ITest test)
        {
            if (test.IsSuite)
            {
                this.BeforeTestSuite(test);
            }
            else
            {
                this.BeforeTestCase(test);
            }
        }

        public void AfterTest(ITest test)
        {
            if (test.IsSuite)
            {
                this.AfterTestSuite(test);
            }
            else
            {
                this.AfterTestCase(test);
            }
        }

        private static void InitializeSingletonFields(IConfigureServices testFixture, IServiceProvider serviceProvider)
        {
            var fields = testFixture
                .GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(field => field.GetCustomAttribute<TestDependencyAttribute>() != null);

            foreach (var field in fields)
            {
                var value = serviceProvider.GetRequiredService(field.FieldType);
                field.SetValue(testFixture, value);
            }
        }

        private void BeforeTestSuite(ITest test)
        {
            var testFixture = test.Fixture;
            if (testFixture is not IConfigureServices configureServices)
            {
                throw new InvalidOperationException($"Test {test.FullName} with fixture {testFixture} do not implement {nameof(IConfigureServices)}");
            }

            var serviceCollection = new ServiceCollection()
                .AddSingleton(configureServices)
                .AddSingleton<IFixtureProvider, FixtureProvider>()
                .AddSingleton<AsyncLocalTestCaseServiceResolver>()
                .AddSingleton(typeof(AsyncLocalTestCaseServiceResolver<>))
                .RegisterFixtures(test);
            configureServices.ConfigureServices(serviceCollection);

            this.serviceProvider = serviceCollection.BuildServiceProvider(Options);
            InitializeSingletonFields(configureServices, this.serviceProvider);
        }

        private void BeforeTestCase(ITest test)
        {
            var sp = this.GetServiceProvider(test);
            var serviceScope = sp.CreateAsyncScope();

            var testMethod = (TestMethod)test;
            var originalMethodInfo = testMethod.Method;
            if (!this.scopes.TryAdd(test, new(serviceScope, originalMethodInfo)))
            {
                throw new InvalidOperationException($"Failed to save original MethodInfo for {test.FullName}");
            }

            var fixture = sp.GetRequiredService<IConfigureServices>();
            testMethod.Method = new UseContainerMethodInfo(originalMethodInfo, serviceScope.ServiceProvider, fixture);

            test.GetFixtureServiceProviderMap().AddScope(fixture, serviceScope.ServiceProvider);
        }

        private void AfterTestCase(ITest test)
        {
            if (!this.scopes.TryRemove(test, out var testData))
            {
                throw new InvalidOperationException($"Failed to get saved TestData for {test.FullName}");
            }

            ((TestMethod)test).Method = testData.OriginalMethodInfo;
            testData.Scope.DisposeSynchronously();

            test.GetFixtureServiceProviderMap().RemoveScope(this.GetServiceProvider(test).GetRequiredService<IConfigureServices>());
        }

        private void AfterTestSuite(ITest test)
        {
            // workaround https://github.com/nunit/nunit/issues/2938
            try
            {
                this.GetServiceProvider(test).DisposeSynchronously();
            }
            catch (Exception ex)
            {
                TestExecutionContext.CurrentContext.CurrentResult.RecordTearDownException(ex);
            }
        }

        [MemberNotNull(nameof(serviceProvider))]
        private ServiceProvider GetServiceProvider(ITest test) => this.serviceProvider ?? throw new InvalidOperationException($"Service provider is not initialized for {test.FullName}");

        private record TestData(AsyncServiceScope Scope, IMethodInfo OriginalMethodInfo);

        private class FixtureProvider : IFixtureProvider
        {
            public FixtureProvider(IConfigureServices fixture) => this.Fixture = fixture;

            public object Fixture { get; }
        }
    }
}
