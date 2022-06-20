using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.Tests.DependencyInjection.ScopeTestMap;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.AfterFixture
{
    [PrimaryConstructor]
    internal partial class AsyncLocalTestCaseServiceResolver
    {
        private readonly IConfigureServices fixture;

        internal TService ResolveService<TService>()
            where TService : notnull =>
            TestExecutionContext.CurrentContext.CurrentTest
                .GetFixtureServiceProviderMap()
                .GetScope(this.fixture)
                .GetRequiredService<TService>();
    }
}
