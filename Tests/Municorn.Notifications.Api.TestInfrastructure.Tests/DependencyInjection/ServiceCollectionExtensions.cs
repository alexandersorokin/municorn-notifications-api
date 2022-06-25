using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework;
using Municorn.Notifications.Api.TestInfrastructure.Logging;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection
{
    internal static class ServiceCollectionExtensions
    {
        internal static IServiceCollection AddFixtureTimeLogger(this IServiceCollection serviceCollection) =>
            serviceCollection
                .AddSingleton<Counter>()
                .AddSingleton<IFixtureOneTimeSetUpService, FixtureOneTimeTimeLogger>();

        internal static IServiceCollection AddTestTimeLogger(this IServiceCollection serviceCollection) =>
            serviceCollection
                .AddSingleton<Counter>()
                .AddScoped<IFixtureSetUpService, TestTimeLogger>();

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
