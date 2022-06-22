using System;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Scopes.Inject;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Scopes
{
    internal sealed class MethodPatcher : IFixtureSetUp, IDisposable
    {
        private readonly IFixtureProvider fixtureProvider;
        private readonly TestAccessor testAccessor;
        private readonly TestMethod testMethod;
        private readonly IMethodInfo originalMethodInfo;

        public MethodPatcher(IFixtureProvider fixtureProvider, TestAccessor testAccessor)
        {
            this.fixtureProvider = fixtureProvider;
            this.testAccessor = testAccessor;
            this.testMethod = this.testAccessor.Test as TestMethod ??
                              throw new InvalidOperationException($"TestCase test should be {nameof(TestMethod)}");
            this.originalMethodInfo = this.testMethod.Method;
        }

        public void Run() => this.testMethod.Method = new UseContainerMethodInfo(this.originalMethodInfo, this.testAccessor.ServiceProvider, this.fixtureProvider);

        public void Dispose() => this.testMethod.Method = this.originalMethodInfo;
    }
}
