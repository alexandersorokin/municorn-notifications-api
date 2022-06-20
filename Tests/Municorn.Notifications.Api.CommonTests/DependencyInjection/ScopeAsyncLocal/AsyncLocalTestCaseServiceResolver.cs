using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.ScopeAsyncLocal
{
    [PrimaryConstructor]
    internal partial class AsyncLocalTestCaseServiceResolver
    {
        private readonly IFixtureProvider fixtureProvider;

        internal TService ResolveService<TService>()
            where TService : notnull =>
            TestExecutionContext.CurrentContext.CurrentTest
                .GetFixtureServiceProviderMap()
                .GetScope(this.fixtureProvider.Fixture)
                .GetRequiredService<TService>();
    }
}
