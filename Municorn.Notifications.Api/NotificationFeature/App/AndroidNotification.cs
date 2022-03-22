using System.Threading.Tasks;

namespace Municorn.Notifications.Api.NotificationFeature.App
{
    [PrimaryConstructor]
    internal partial class AndroidNotification : INotification
    {
        private readonly AndroidNotificationSender notificationSender;
        private readonly AndroidNotificationData notificationData;

        public async Task<SendResult> Send()
        {
            return await this.notificationSender.Send(this.notificationData).ConfigureAwait(false);
        }
    }
}
