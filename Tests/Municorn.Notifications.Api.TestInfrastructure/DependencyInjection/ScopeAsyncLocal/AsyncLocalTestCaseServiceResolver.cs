using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.ScopeAsyncLocal
{
    public class AsyncLocalTestCaseServiceResolver
    {
        private readonly IFixtureProvider fixtureProvider;

        internal AsyncLocalTestCaseServiceResolver(IFixtureProvider fixtureProvider) => this.fixtureProvider = fixtureProvider;

        public TService GetRequiredService<TService>()
            where TService : notnull =>
            TestExecutionContext.CurrentContext.CurrentTest
                .GetServiceProvider(this.fixtureProvider.Fixture)
                .GetRequiredService<TService>();
    }
}
