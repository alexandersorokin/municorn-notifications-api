namespace Municorn.Notifications.Api.Tests.DependencyInjection.ScopeTestMap.AsyncLocal
{
    internal static class FixtureExtensions
    {
        internal static TService ResolveService<TService>(this object fixture)
            where TService : notnull =>
            new AsyncLocalTestCaseServiceResolver(new FixtureProvider(fixture)).ResolveService<TService>();

        private class FixtureProvider : IFixtureProvider
        {
            public FixtureProvider(object fixture) => this.Fixture = fixture;

            public object Fixture { get; }
        }
    }
}