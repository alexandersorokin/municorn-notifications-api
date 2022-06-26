using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureBuilder;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.TestCommunication;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Commands;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureBuilder
{
    [TestFixtureInjectable]
    [TestMethodInjectionModule]
    [TestCommunicationModule]
    [CounterScopedModule]
    internal class Dispose_Scope_After_Wraps_Should
    {
        [Test]
        [Repeat(2)]
        [EnsureNotDisposed]
        public void Case([InjectDependency] Counter service)
        {
            service.Increment();
            service.Should().NotBeNull();
        }

        [TestCase(10)]
        [TestCase(11)]
        [EnsureNotDisposed]
        [Repeat(2)]
        public void Cases(int value, [InjectDependency] Counter service)
        {
            service.Increment();
            service.Should().NotBeNull();
        }

        [AttributeUsage(AttributeTargets.Method)]
        private sealed class EnsureNotDisposedAttribute : NUnitAttribute, IWrapSetUpTearDown
        {
            public TestCommand Wrap(TestCommand command) => new EnsureNotDisposedCommand(command);

            private class EnsureNotDisposedCommand : TestCommand
            {
                private readonly TestCommand command;

                public EnsureNotDisposedCommand(TestCommand command)
                    : base(command.Test) => this.command = command;

                public override TestResult Execute(TestExecutionContext context)
                {
                    var result = this.command.Execute(context);

                    context.CurrentTest
                        .GetServiceProvider(context.TestObject)
                        .GetRequiredService<Counter>()
                        .Value
                        .Should()
                        .Be(1);

                    return result;
                }
            }
        }
    }
}
