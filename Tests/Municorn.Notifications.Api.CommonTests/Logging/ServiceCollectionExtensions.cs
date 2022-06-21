using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.Tests.Logging
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
                .AddSingleton<ITextWriterProvider, NUnitTextWriterProvider>()
                .AddSingleton<ILog, TextWriterLog>();
    }
}
