using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Vostok.Logging.Abstractions;
using Vostok.Logging.Microsoft;

namespace Municorn.Notifications.Api.Tests.IntegrationTests
{
    internal static class ServiceCollectionLogExtensions
    {
        internal static IServiceCollection RegisterMicrosoftLogger(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddScoped<ILoggerProvider, VostokLoggerProvider>()
                .AddScoped<ILoggerFactory, LoggerFactory>()
                .AddScoped(typeof(ILogger<>), typeof(Logger<>));
        }

        internal static IServiceCollection RegisterLogSniffer(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton(NUnitLog.CreateContextual())
                .AddScoped<LogMessageContainer>()
                .AddScoped<SniffLog>()
                .AddScoped<ILog, SniffCompositeLog>();
        }
    }
}
