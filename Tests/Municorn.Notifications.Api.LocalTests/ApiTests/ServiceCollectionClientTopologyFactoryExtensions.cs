using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.Tests.ApiTests
{
    internal static class ServiceCollectionClientTopologyFactoryExtensions
    {
        internal static IServiceCollection RegisterClientTopologyFactory(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .RegisterSetUpFixtureProvider(TestExecutionContext.CurrentContext.CurrentTest)
                .AddSingleton<LocalApiRunner>()
                .AddSingleton<LocalApiRunnerCache>()
                .AddSingleton<IClientTopologyFactory, LocalApiTopologyFactory>();
        }
    }
}
