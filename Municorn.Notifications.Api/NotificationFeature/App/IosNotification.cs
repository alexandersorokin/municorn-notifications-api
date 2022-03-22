using System.Threading.Tasks;

namespace Municorn.Notifications.Api.NotificationFeature.App
{
    [PrimaryConstructor]
    internal partial class IosNotification : INotification
    {
        private readonly IosNotificationSender notificationSender;
        private readonly IosNotificationData notificationData;

        public async Task<SendResult> Send()
        {
            return await this.notificationSender.Send(this.notificationData).ConfigureAwait(false);
        }
    }
}
