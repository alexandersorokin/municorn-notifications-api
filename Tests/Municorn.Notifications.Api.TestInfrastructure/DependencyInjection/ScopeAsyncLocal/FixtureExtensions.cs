namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.ScopeAsyncLocal
{
    public static class FixtureExtensions
    {
        public static TService ResolveService<TService>(this object fixture)
            where TService : notnull =>
            new AsyncLocalTestCaseServiceResolver(new FixtureProvider(fixture)).ResolveService<TService>();
    }
}