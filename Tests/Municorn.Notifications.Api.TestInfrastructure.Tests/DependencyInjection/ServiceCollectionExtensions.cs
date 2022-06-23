using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection
{
    internal static class ServiceCollectionExtensions
    {
        internal static IServiceCollection AddFixtureTimeLogger(this IServiceCollection serviceCollection) =>
            serviceCollection
                .AddSingleton<Counter>()
                .AddSingleton<IFixtureOneTimeSetUp, FixtureTimeLogger>();

        internal static IServiceCollection AddTestTimeLogger(this IServiceCollection serviceCollection) =>
            serviceCollection
                .AddSingleton<Counter>()
                .AddScoped<IFixtureSetUp, TestTimeLogger>();
    }
}
