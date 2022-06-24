using Microsoft.Extensions.DependencyInjection;

namespace Municorn.Notifications.Api.Tests.ApiTests
{
    internal static class ServiceCollectionClientTopologyFactoryExtensions
    {
        internal static IServiceCollection AddClientTopologyFactory(this IServiceCollection serviceCollection) =>
            serviceCollection
                .AddSingleton<IClientTopologyFactory, RemoteApiTopologyFactory>();
    }
}
