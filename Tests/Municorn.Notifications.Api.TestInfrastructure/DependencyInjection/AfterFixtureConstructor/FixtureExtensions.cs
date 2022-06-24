using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Communication.AsyncLocal;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor
{
    public static class FixtureExtensions
    {
        public static TService GetRequiredService<TService>(this ITestFixture fixture)
            where TService : notnull =>
            new AsyncLocalServiceProvider<TService>(new AsyncLocalServiceProvider(new FixtureProvider(fixture))).Value;
    }
}