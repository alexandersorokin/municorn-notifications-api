using System;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Scopes.Inject
{
    internal sealed class MethodPatcher : IFixtureSetUp, IDisposable
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IFixtureProvider fixtureProvider;
        private readonly TestMethod testMethod;
        private readonly IMethodInfo originalMethodInfo;

        public MethodPatcher(IServiceProvider serviceProvider, IFixtureProvider fixtureProvider, TestAccessor testAccessor)
        {
            this.serviceProvider = serviceProvider;
            this.fixtureProvider = fixtureProvider;
            this.testMethod = testAccessor.Test as TestMethod ??
                              throw new InvalidOperationException($"TestCase test should be {nameof(TestMethod)}");
            this.originalMethodInfo = this.testMethod.Method;
        }

        public void Run() => this.testMethod.Method = new UseContainerMethodInfo(this.originalMethodInfo, this.serviceProvider, this.fixtureProvider);

        public void Dispose() => this.testMethod.Method = this.originalMethodInfo;
    }
}
