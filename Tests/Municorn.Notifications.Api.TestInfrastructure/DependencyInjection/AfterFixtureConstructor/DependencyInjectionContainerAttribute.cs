using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Scopes;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor
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
                this.GetServiceProvider(test).GetRequiredService<TestActionMethodManager>().BeforeTestCase(test);
            }
        }

        public void AfterTest(ITest test)
        {
            var provider = this.GetServiceProvider(test);
            if (test.IsSuite)
            {
                AfterTestSuite(provider);
            }
            else
            {
                provider.GetRequiredService<TestActionMethodManager>().AfterTestCase(test);
            }
        }

        private static ServiceProvider ConfigureTestFixture(ITest test, ITestFixture fixture)
        {
            var serviceCollection = new ServiceCollection()
                .AddSingleton<IFixtureProvider>(new FixtureProvider(fixture))
                .AddSingleton(test)
                .AddTestActionManager()
                .AddAsyncLocal()
                .AddFixtures(test)
                .AddFixtureAutoMethods()
                .AddFixtureModules(test.TypeInfo ?? throw new InvalidOperationException("No typeInfo is found at container configuration"));
            fixture.ConfigureServices(serviceCollection);

            var sp = serviceCollection.BuildServiceProvider(Options);
            sp.GetRequiredService<FixtureOneTimeSetUpRunner>().Run();
            return sp;
        }

        private static void AfterTestSuite(IAsyncDisposable provider)
        {
            // workaround https://github.com/nunit/nunit/issues/2938
            try
            {
                provider.DisposeAsync().AsTask().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                TestExecutionContext.CurrentContext.CurrentResult.RecordTearDownException(ex);
            }
        }

        private void BeforeTestSuite(ITest test)
        {
            var fixture = test.Fixture;
            if (fixture is not ITestFixture testFixture)
            {
                throw new InvalidOperationException($"Test {test.FullName} with fixture {fixture} do not implement {nameof(ITestFixture)}");
            }

            this.serviceProvider = ConfigureTestFixture(test, testFixture);
        }

        [MemberNotNull(nameof(serviceProvider))]
        private ServiceProvider GetServiceProvider(ITest test) => this.serviceProvider ?? throw new InvalidOperationException($"Service provider is not initialized for {test.FullName}");
    }
}
