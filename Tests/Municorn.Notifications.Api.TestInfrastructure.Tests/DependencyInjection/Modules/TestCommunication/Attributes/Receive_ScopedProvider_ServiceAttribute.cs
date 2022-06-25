using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.TestCommunication;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal.Commands;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Modules.TestCommunication.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    internal sealed class Receive_ScopedProvider_ServiceAttribute : NUnitAttribute, IWrapTestMethod
    {
        public TestCommand Wrap(TestCommand command)
        {
            return new EnsureLogResolved(command);
        }

        private class EnsureLogResolved : BeforeTestCommand
        {
            public EnsureLogResolved(TestCommand innerCommand)
                : base(innerCommand)
            {
                this.BeforeTest = context => context.CurrentTest
                    .GetServiceProvider(context.TestObject)
                    .GetRequiredService<SilentLog>()
                    .Should()
                    .NotBeNull();
            }
        }
    }
}
