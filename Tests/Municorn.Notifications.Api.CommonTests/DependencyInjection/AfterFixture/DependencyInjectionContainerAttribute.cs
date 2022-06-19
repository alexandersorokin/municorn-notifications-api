﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.Tests.DependencyInjection.Scope;
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
            test.GetFixtureServiceProviderMap().AddScope(sp.GetRequiredService<IConfigureServices>(), serviceScope);
        }

        private void AfterTestCase(ITest test)
        {
            var fixture = this.GetServiceProvider(test).GetRequiredService<IConfigureServices>();
            var map = test.GetFixtureServiceProviderMap();
            map.GetScope(fixture).DisposeSynchronously();
            map.RemoveScope(fixture);
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
    }
}