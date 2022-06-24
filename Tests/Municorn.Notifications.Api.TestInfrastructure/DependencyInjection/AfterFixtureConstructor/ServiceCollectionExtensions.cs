using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFieldInjection(this IServiceCollection serviceCollection, IFixtureServiceProviderFramework fixture) =>
            serviceCollection.AddFieldInjection(fixture.GetType());
    }
}
