using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.Tests.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Interface)]
    internal sealed class DependencyInjectionContainerAttribute : NUnitAttribute, ITestAction
    {
        private static readonly ServiceProviderOptions Options = new()
        {
            ValidateOnBuild = true,
            ValidateScopes = true,
        };

        private FixtureData? fixtureData;

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
                throw new InvalidOperationException($"Test {test} with fixture {testFixture} do not implement {nameof(IConfigureServices)}");
            }

            ServiceCollection serviceCollection = new();
            configureServices.ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider(Options);
            InitializeSingletonFields(configureServices, serviceProvider);

            this.fixtureData = new(configureServices, serviceProvider);
        }

        private void BeforeTestCase(ITest test)
        {
            var (fixture, serviceProvider) = this.EnsureFixtureDataInitialized(test);
#pragma warning disable CA2000 // Dispose objects before losing scope
            var serviceScope = serviceProvider.CreateAsyncScope();
#pragma warning restore CA2000 // Dispose objects before losing scope
            fixture.AddScope(test, serviceScope);
        }

        private void AfterTestCase(ITest test)
        {
            this.EnsureFixtureDataInitialized(test)
                .Fixture
                .DisposeScope(test);
        }

        private void AfterTestSuite(ITest test)
        {
            // workaround https://github.com/nunit/nunit/issues/2938
            try
            {
                this.EnsureFixtureDataInitialized(test)
                    .ServiceProvider
                    .DisposeSynchronously();
            }
            catch (Exception ex)
            {
                TestExecutionContext.CurrentContext.CurrentResult.RecordTearDownException(ex);
            }
        }

        [MemberNotNull(nameof(fixtureData))]
        private FixtureData EnsureFixtureDataInitialized(ITest test)
        {
            return this.fixtureData is { } result
                ? result
                : throw new InvalidOperationException($"Fixture data is not initialized for {test}");
        }

        private record FixtureData(IConfigureServices Fixture, ServiceProvider ServiceProvider);
    }
}
