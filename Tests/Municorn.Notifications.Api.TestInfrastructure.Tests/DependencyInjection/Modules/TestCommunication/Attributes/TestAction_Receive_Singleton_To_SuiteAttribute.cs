using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.TestCommunication;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Modules.TestCommunication.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    internal sealed class TestAction_Receive_Singleton_To_SuiteAttribute : NUnitAttribute, ITestAction
    {
        private object? testFixture;

        public ActionTargets Targets => ActionTargets.Suite;

        public void BeforeTest(ITest test)
        {
            this.testFixture = test.Fixture;
            this.EnsureHaveContainer(test);
        }

        public void AfterTest(ITest test)
        {
            this.EnsureHaveContainer(test);
        }

        private void EnsureHaveContainer(ITest test) =>
            test
                .GetServiceProvider(this.testFixture ?? throw new InvalidOperationException("Fixture is not found"))
                .GetRequiredService<SilentLog>()
                .Should()
                .NotBeNull();
    }
}
