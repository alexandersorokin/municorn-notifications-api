using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.NotificationFeature.App;
using Municorn.Notifications.Api.NotificationFeature.Data;

namespace Municorn.Notifications.Api.NotificationFeature.View
{
    public static class ServiceCollectionConverterExtensions
    {
        public static IServiceCollection RegisterRequestConverter(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                    .AddSingleton<NotificationStatusRepository>()
                    .AddSingleton<INotificationSender<AndroidNotificationData>, AndroidNotificationSender>()
                    .AddSingleton<INotificationSender<IosNotificationData>, IosNotificationSender>()
                    .AddSingleton<INotificationRequestConverter, NotificationRequestConverter>();
        }
    }
}
