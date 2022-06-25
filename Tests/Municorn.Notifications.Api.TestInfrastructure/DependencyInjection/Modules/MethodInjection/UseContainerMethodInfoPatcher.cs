using System;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection
{
    internal sealed class UseContainerMethodInfoPatcher : IFixtureSetUpService, IDisposable
    {
        private readonly UseContainerMethodInfoFactory useContainerMethodInfoFactory;
        private readonly TestMethod testMethod;

        private IMethodInfo? originalMethodInfo;

        public UseContainerMethodInfoPatcher(UseContainerMethodInfoFactory useContainerMethodInfoFactory, TestAccessor testAccessor)
        {
            this.useContainerMethodInfoFactory = useContainerMethodInfoFactory;
            this.testMethod = testAccessor.Test as TestMethod ??
                              throw new InvalidOperationException($"TestCase test should be {nameof(TestMethod)}");
        }

        public void Run()
        {
            this.originalMethodInfo = this.testMethod.Method;
            this.testMethod.Method = this.useContainerMethodInfoFactory.Create(this.originalMethodInfo);
        }

        public void Dispose() => this.testMethod.Method = this.originalMethodInfo ?? this.testMethod.Method;
    }
}
