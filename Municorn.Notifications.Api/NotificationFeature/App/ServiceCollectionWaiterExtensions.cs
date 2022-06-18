using Microsoft.Extensions.DependencyInjection;

namespace Municorn.Notifications.Api.NotificationFeature.App
{
    public static class ServiceCollectionWaiterExtensions
    {
        public static IServiceCollection RegisterWaiter(this IServiceCollection serviceCollection) =>
            serviceCollection
                .AddSingleton<ThreadSafeRandomNumberGenerator>()
                .AddSingleton<Waiter>();
    }
}
