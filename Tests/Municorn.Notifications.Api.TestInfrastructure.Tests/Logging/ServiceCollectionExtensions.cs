using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.Logging;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.Logging
{
    internal static class ServiceCollectionExtensions
    {
        internal static IServiceCollection AddBoundLog(this IServiceCollection serviceCollection) =>
            serviceCollection
                .AddSingleton(TestContext.Out)
                .AddSingleton<ITextWriterProvider, AdHocTextWriterProvider>()
                .AddSingleton<ILog, TextWriterLog>();

        internal static IServiceCollection AddContextualLog(this IServiceCollection serviceCollection) =>
            serviceCollection
                .AddSingleton<ITextWriterProvider, NUnitAsyncLocalTextWriterProvider>()
                .AddSingleton<ILog, TextWriterLog>();
    }
}
