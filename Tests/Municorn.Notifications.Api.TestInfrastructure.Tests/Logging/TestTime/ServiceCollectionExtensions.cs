using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework;
using Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.Logging.TestTime
{
    internal static class ServiceCollectionExtensions
    {
        internal static IServiceCollection AddTestTimeLogger(this IServiceCollection serviceCollection) =>
            serviceCollection
                .AddSingleton<Counter>()
                .AddScoped<IFixtureSetUpService, TestTimeLogger>();
    }
}
