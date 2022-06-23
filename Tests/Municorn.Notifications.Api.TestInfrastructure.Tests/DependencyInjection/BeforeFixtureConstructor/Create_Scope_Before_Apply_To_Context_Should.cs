using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.BeforeFixtureConstructor;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Communication;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.BeforeFixtureConstructor
{
    [TestFixtureInjectable]
    [TestMethodInjectionModule]
    [TestCommunicationModule]
    [TimeLoggerModule]
    internal class Create_Scope_Before_Apply_To_Context_Should
    {
        [Test]
        [Repeat(2)]
        [ApplyToContext]
        public void Case([InjectDependency] IFixtureSetUp service)
        {
            service.Should().NotBeNull();
        }

        [TestCase(10)]
        [TestCase(11)]
        [ApplyToContext]
        [Repeat(2)]
        public void Cases(int value, [InjectDependency] IFixtureSetUp service)
        {
            service.Should().NotBeNull();
        }

        [AttributeUsage(AttributeTargets.Method)]
        private sealed class ApplyToContextAttribute : NUnitAttribute, IApplyToContext
        {
            public void ApplyToContext(TestExecutionContext context)
            {
                var test = context.CurrentTest;
                if (!test.IsSuite)
                {
                    test
                        .GetServiceProvider(context.TestObject)
                        .GetRequiredService<IFixtureSetUp>()
                        .Should()
                        .NotBeNull();
                }
            }
        }
    }
}
