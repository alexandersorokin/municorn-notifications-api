using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.AfterFixtureConstructor.Tests.Communication
{
    internal sealed class TestActionLoggerAttribute : NUnitAttribute, ITestAction
    {
        private object? testFixture;

        public ActionTargets Targets => ActionTargets.Test;

        public void BeforeTest(ITest test)
        {
            this.testFixture = test.Fixture;
            this.EnsureHaveContainer(test);
        }

        public void AfterTest(ITest test)
        {
            this.EnsureHaveContainer(test);
        }

        private void EnsureHaveContainer(ITest test)
        {
            var service = test
                .GetFixtureServiceProviderMap()
                .GetScope(this.testFixture ?? throw new InvalidOperationException("Fixture is not found"))
                .GetRequiredService<ILog>();
            service.Should().NotBeNull();
        }
    }
}
