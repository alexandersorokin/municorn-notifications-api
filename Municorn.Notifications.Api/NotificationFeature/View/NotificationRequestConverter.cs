using Municorn.Notifications.Api.NotificationFeature.App;

namespace Municorn.Notifications.Api.NotificationFeature.View
{
    [PrimaryConstructor]
    public partial class NotificationRequestConverter : INotificationRequestConverter
    {
        private readonly INotificationSender<IosNotificationData> iosNotificationSender;
        private readonly INotificationSender<AndroidNotificationData> androidNotificationSender;

        public INotification Visit(IosSendNotificationRequest request)
        {
            var iosNotification = new IosNotificationData(request.PushToken, request.Alert)
            {
                IsBackground = request.IsBackground,
                Priority = request.Priority,
            };
            return Notification.Create(iosNotification, this.iosNotificationSender);
        }

        public INotification Visit(AndroidSendNotificationRequest request)
        {
            var androidNotification = new AndroidNotificationData(request.DeviceToken, request.Message, request.Title)
            {
                Condition = request.Condition,
            };
            return Notification.Create(androidNotification, this.androidNotificationSender);
        }
    }
}
