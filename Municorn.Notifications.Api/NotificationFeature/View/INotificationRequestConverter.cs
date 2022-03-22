using Municorn.Notifications.Api.NotificationFeature.App;

namespace Municorn.Notifications.Api.NotificationFeature.View
{
    public interface INotificationRequestConverter : INotificationRequestVisitor<INotification>
    {
    }
}