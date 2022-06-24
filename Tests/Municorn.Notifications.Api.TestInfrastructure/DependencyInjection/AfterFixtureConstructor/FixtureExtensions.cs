using AsyncLocalServiceProvider = Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.TestCommunication.AsyncLocal.AsyncLocalServiceProvider;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor
{
    public static class FixtureExtensions
    {
        public static TService GetRequiredService<TService>(this IFixtureServiceProvider fixture)
            where TService : notnull =>
            new Modules.TestCommunication.AsyncLocal.AsyncLocalServiceProvider<TService>(new AsyncLocalServiceProvider(new FixtureProvider(fixture))).Value;
    }
}