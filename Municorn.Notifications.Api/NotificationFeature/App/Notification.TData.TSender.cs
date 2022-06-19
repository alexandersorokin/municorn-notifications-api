using System.Threading.Tasks;

namespace Municorn.Notifications.Api.NotificationFeature.App
{
    [PrimaryConstructor]
    internal partial class Notification<TData, TSender> : INotification
        where TSender : INotificationSender<TData>
    {
        private readonly TData notificationData;
        private readonly TSender notificationSender;

        public async Task<SendResult> Send() => await this.notificationSender.Send(this.notificationData).ConfigureAwait(false);
    }
}
