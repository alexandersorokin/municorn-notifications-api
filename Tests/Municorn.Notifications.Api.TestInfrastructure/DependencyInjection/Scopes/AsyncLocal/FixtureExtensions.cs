using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Scopes.AsyncLocal
{
    public static class FixtureExtensions
    {
        public static TService GetRequiredService<TService>(this ITestFixture fixture)
            where TService : notnull =>
            new AsyncLocalServiceProvider<TService>(new(new FixtureProvider(fixture))).Value;
    }
}