namespace Municorn.Notifications.Api.NotificationFeature.App
{
    internal static class Notification
    {
        internal static INotification Create<TData, TSender>(TData data, TSender sender)
            where TSender : INotificationSender<TData>
        {
            return new Notification<TData, TSender>(data, sender);
        }
    }
}
