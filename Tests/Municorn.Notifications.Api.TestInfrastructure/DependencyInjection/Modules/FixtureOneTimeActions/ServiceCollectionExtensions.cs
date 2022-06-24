using Microsoft.Extensions.DependencyInjection;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FixtureOneTimeActions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFixtureOneTimeActions(this IServiceCollection serviceCollection) =>
            serviceCollection.AddSingleton<IFixtureOneTimeSetUpService, FixtureOneTimeActionRunner>();
    }
}
