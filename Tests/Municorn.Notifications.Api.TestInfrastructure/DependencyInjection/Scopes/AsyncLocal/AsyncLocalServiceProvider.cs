using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Scopes.AsyncLocal
{
    public class AsyncLocalServiceProvider
    {
        private readonly IFixtureProvider fixtureProvider;

        internal AsyncLocalServiceProvider(IFixtureProvider fixtureProvider) => this.fixtureProvider = fixtureProvider;

        public TService GetRequiredService<TService>()
            where TService : notnull =>
            TestExecutionContext.CurrentContext.CurrentTest
                .GetServiceProvider(this.fixtureProvider.Fixture)
                .GetRequiredService<TService>();
    }
}
