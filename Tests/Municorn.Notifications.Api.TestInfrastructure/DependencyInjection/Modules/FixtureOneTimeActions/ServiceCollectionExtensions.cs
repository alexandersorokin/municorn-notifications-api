using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FixtureOneTimeActions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFixtureOneTimeActions(this IServiceCollection serviceCollection) =>
            serviceCollection.AddSingleton<IFixtureOneTimeSetUpAsyncService, FixtureOneTimeActionRunner>();
    }
}
