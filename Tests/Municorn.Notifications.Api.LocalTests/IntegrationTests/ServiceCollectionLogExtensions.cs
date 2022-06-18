using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Municorn.Notifications.Api.Tests.Log;
using Vostok.Logging.Abstractions;
using Vostok.Logging.Microsoft;

namespace Municorn.Notifications.Api.Tests.IntegrationTests
{
    internal static class ServiceCollectionLogExtensions
    {
        internal static IServiceCollection RegisterMicrosoftLogger(this IServiceCollection serviceCollection) =>
            serviceCollection
                .AddScoped<ILoggerProvider, VostokLoggerProvider>()
                .AddScoped<ILoggerFactory, LoggerFactory>()
                .AddScoped(typeof(ILogger<>), typeof(Logger<>));

        internal static IServiceCollection RegisterLogSniffer(this IServiceCollection serviceCollection) =>
            serviceCollection
                .AddSingleton<ITextWriterProvider, NUnitTextWriterProvider>()
                .AddSingleton<TextWriterLog>()
                .AddScoped<LogMessageContainer>()
                .AddScoped<SniffLog>()
                .AddScoped<ILog, SniffCompositeLog>();

        private class SniffCompositeLog : CompositeLog
        {
            public SniffCompositeLog(TextWriterLog textWriterLog, SniffLog sniffLog)
                : base(textWriterLog, sniffLog)
            {
            }
        }
    }
}
