using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Abstractions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor
{
    [AttributeUsage(AttributeTargets.Interface)]
    internal sealed class ServiceProviderFrameworkAttribute : NUnitAttribute, ITestAction
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
            if (test.IsSuite)
            {
                this.AfterTestSuite();
            }
            else
            {
                this.GetServiceProvider(test).GetRequiredService<TestActionMethodManager>().AfterTestCase(test);
            }
        }

        private void AfterTestSuite()
        {
            // workaround https://github.com/nunit/nunit/issues/2938
            try
            {
                this.serviceProvider?.DisposeAsync().AsTask().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                TestExecutionContext.CurrentContext.CurrentResult.RecordTearDownException(ex);
            }
        }

        private void BeforeTestSuite(ITest test)
        {
            var fixture = test.Fixture;
            if (fixture is not IFixtureServiceProviderFramework testFixture)
            {
                throw new InvalidOperationException($"Test {test.FullName} with fixture {fixture} do not implement {nameof(IFixtureServiceProviderFramework)}");
            }

            var serviceCollection = new ServiceCollection()
                .AddSingleton<IFixtureProvider>(new FixtureProvider(testFixture))
                .AddSingleton(test)
                .AddFixtures(test)
                .AddFixtureAutoMethods()
                .AddFixtureModules(test.TypeInfo ?? throw new InvalidOperationException("No typeInfo is found at service collection configuration"));
            testFixture.ConfigureServices(serviceCollection);

            this.serviceProvider = serviceCollection.BuildServiceProvider(Options);
            this.serviceProvider.GetRequiredService<FixtureOneTimeSetUpRunner>().Run();
        }

        [MemberNotNull(nameof(serviceProvider))]
        private ServiceProvider GetServiceProvider(ITest test) => this.serviceProvider ?? throw new InvalidOperationException($"Service provider is not initialized for {test.FullName}");
    }
}
