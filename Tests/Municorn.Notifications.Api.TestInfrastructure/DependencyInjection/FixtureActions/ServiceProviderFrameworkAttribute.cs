using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Abstractions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureActions
{
    [AttributeUsage(AttributeTargets.Interface)]
    internal sealed class ServiceProviderFrameworkAttribute : NUnitAttribute, ITestAction
    {
        private FixtureServiceProviderFramework? framework;

        public ActionTargets Targets => ActionTargets.Suite | ActionTargets.Test;

        public void BeforeTest(ITest test)
        {
            if (test.IsSuite)
            {
                this.BeforeTestSuite(test);
            }
            else
            {
                this.GetFramework(test).RunSetUp(test).GetAwaiter().GetResult();
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
                this.GetFramework(test).RunTearDown(test).GetAwaiter().GetResult();
            }
        }

        private void AfterTestSuite()
        {
            // workaround https://github.com/nunit/nunit/issues/2938
            try
            {
                this.framework?.DisposeAsync().AsTask().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                TestExecutionContext.CurrentContext.CurrentResult.RecordTearDownException(ex);
            }
        }

        private void BeforeTestSuite(ITest test)
        {
            var fixture = test.Fixture;
            if (fixture is not IFixtureWithServiceProviderFramework testFixture)
            {
                throw new InvalidOperationException($"Test {test.FullName} with fixture {fixture} does not implement {nameof(IFixtureWithServiceProviderFramework)}");
            }

            this.framework = new(serviceCollection =>
            {
                serviceCollection
                    .AddFixtureProvider(testFixture)
                    .AddSingleton(test)
                    .AddFixtures(test)
                    .AddFixtureServiceCollectionModuleAttributes(test.TypeInfo?.Type ?? throw new InvalidOperationException("No TypeInfo is found at service collection configuration"));
                testFixture.ConfigureServices(serviceCollection);
            });

            this.framework.RunOneTimeSetUp().GetAwaiter().GetResult();
        }

        [MemberNotNull(nameof(framework))]
        private FixtureServiceProviderFramework GetFramework(ITest test) => this.framework ?? throw new InvalidOperationException($"Service provider framework is not initialized for {test.FullName}");
    }
}
