using JetBrains.Annotations;

namespace Municorn.Notifications.Api.NotificationFeature.App
{
    public interface INotificationData
    {
        [MustUseReturnValue]
        string SerializeToString();
    }
}
