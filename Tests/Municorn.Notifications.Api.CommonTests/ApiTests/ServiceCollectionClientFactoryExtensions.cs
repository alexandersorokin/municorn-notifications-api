using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.Logging;

namespace Municorn.Notifications.Api.Tests.ApiTests
{
    internal static class ServiceCollectionClientFactoryExtensions
    {
        internal static IServiceCollection AddClientFactory(this IServiceCollection serviceCollection) =>
            serviceCollection
                .AddContextualLog()
                .AddClientTopologyFactory()
                .AddSingleton<ClientFactory>();
    }
}
