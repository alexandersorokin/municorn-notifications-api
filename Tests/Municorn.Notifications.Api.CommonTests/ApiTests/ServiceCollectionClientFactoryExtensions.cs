using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.Tests.Log;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.Tests.ApiTests
{
    internal static class ServiceCollectionClientFactoryExtensions
    {
        internal static IServiceCollection RegisterClientFactory(this IServiceCollection serviceCollection) =>
            serviceCollection
                .AddSingleton<ITextWriterProvider, NUnitTextWriterProvider>()
                .AddSingleton<ILog, TextWriterLog>()
                .RegisterClientTopologyFactory()
                .AddSingleton<ClientFactory>();
    }
}
