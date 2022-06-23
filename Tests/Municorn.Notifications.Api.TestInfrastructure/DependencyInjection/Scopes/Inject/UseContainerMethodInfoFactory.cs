using System;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Scopes.Inject
{
    [PrimaryConstructor]
    internal partial class UseContainerMethodInfoFactory
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ITestFixtureProvider testFixtureProvider;

        public UseContainerMethodInfo Create(IMethodInfo methodInfo) =>
            new(methodInfo, this.serviceProvider, this.testFixtureProvider);
    }
}
