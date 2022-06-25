using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework;

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
