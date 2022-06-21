using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.ScopeAsyncLocal
{
    [PrimaryConstructor]
    public partial class AsyncLocalTestCaseServiceResolver
    {
        private readonly IFixtureProvider fixtureProvider;

        public TService ResolveService<TService>()
            where TService : notnull =>
            TestExecutionContext.CurrentContext.CurrentTest
                .GetFixtureServiceProviderMap()
                .GetScope(this.fixtureProvider.Fixture)
                .GetRequiredService<TService>();
    }
}
