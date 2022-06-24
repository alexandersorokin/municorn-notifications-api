using Microsoft.Extensions.DependencyInjection;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTestMethodInjection(this IServiceCollection serviceCollection) =>
            serviceCollection
                .AddScoped<UseContainerMethodInfoFactory>()
                .AddScoped<IFixtureSetUpService, UseContainerMethodInfoPatcher>();
    }
}
