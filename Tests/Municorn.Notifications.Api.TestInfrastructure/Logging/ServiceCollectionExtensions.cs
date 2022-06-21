using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Logging
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBoundLog(this IServiceCollection serviceCollection) =>
            serviceCollection
                .AddSingleton(TestContext.Out)
                .AddSingleton<ITextWriterProvider, AdHocTextWriterProvider>()
                .AddSingleton<ILog, TextWriterLog>();

        public static IServiceCollection AddContextualLog(this IServiceCollection serviceCollection) =>
            serviceCollection
                .AddSingleton<ITextWriterProvider, NUnitTextWriterProvider>()
                .AddSingleton<ILog, TextWriterLog>();
    }
}
