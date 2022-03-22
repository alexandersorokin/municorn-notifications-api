using Municorn.Notifications.Api.NotificationFeature.App;

namespace Municorn.Notifications.Api.NotificationFeature.View
{
    [PrimaryConstructor]
    public partial class NotificationRequestConverter : INotificationRequestConverter
    {
        private readonly IosNotificationSender iosNotificationSender;
        private readonly AndroidNotificationSender androidNotificationSender;

        public INotification Visit(IosSendNotificationRequest request)
        {
            var iosNotification = new IosNotificationData(request.PushToken, request.Alert)
            {
                IsBackground = request.IsBackground,
                Priority = request.Priority,
            };
            return new IosNotification(this.iosNotificationSender, iosNotification);
        }

        public INotification Visit(AndroidSendNotificationRequest request)
        {
            var androidNotification = new AndroidNotificationData(request.DeviceToken, request.Message, request.Title)
            {
                Condition = request.Condition,
            };
            return new AndroidNotification(this.androidNotificationSender, androidNotification);
        }
    }
}
