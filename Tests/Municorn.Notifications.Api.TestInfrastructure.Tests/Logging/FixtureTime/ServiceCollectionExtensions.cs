using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework;
using Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.Logging.FixtureTime
{
    internal static class ServiceCollectionExtensions
    {
        internal static IServiceCollection AddFixtureTimeLogger(this IServiceCollection serviceCollection) =>
            serviceCollection
                .AddSingleton<Counter>()
                .AddSingleton<IFixtureOneTimeSetUpService, FixtureOneTimeTimeLogger>();
    }
}
