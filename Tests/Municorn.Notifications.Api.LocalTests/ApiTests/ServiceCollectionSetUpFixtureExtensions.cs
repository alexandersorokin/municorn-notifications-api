using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.Tests.ApiTests
{
    internal static class ServiceCollectionSetUpFixtureExtensions
    {
        internal static IServiceCollection RegisterSetUpFixtureProvider(
            this IServiceCollection serviceCollection,
            ITest currentTest)
        {
            return serviceCollection
                .AddSingleton(currentTest)
                .AddSingleton(typeof(SetupFixtureProvider<>));
        }
    }
}
