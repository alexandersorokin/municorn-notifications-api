using Microsoft.Extensions.DependencyInjection;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.TestCommunication
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTestCommunication(this IServiceCollection serviceCollection) =>
            serviceCollection
                .AddSingleton<IAsyncLocalServiceProvider, AsyncLocalServiceProvider>()
                .AddSingleton(typeof(IAsyncLocalServiceProvider<>), typeof(AsyncLocalServiceProvider<>))
                .AddSingleton<IFixtureOneTimeSetUp, MapProviderSaver>()
                .AddScoped<IFixtureSetUp, MapScopeSaver>();
    }
}
