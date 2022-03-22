using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Municorn.Notifications.Api.NotificationFeature.Data;

namespace Municorn.Notifications.Api.NotificationFeature.App
{
    [PrimaryConstructor]
    public partial class AndroidNotificationSender : INotificationSender<AndroidNotificationData>
    {
        private readonly ILogger<AndroidNotificationSender> logger;
        private readonly NotificationStatusRepository statusRepository;
        private readonly Waiter waiter;
        private int sendCounter;

        public async Task<SendResult> Send(AndroidNotificationData request)
        {
            await this.waiter.Wait().ConfigureAwait(false);

            this.logger.NotificationSent("AndroidSender", request);

            var notificationId = Guid.NewGuid();
            var delivered = Interlocked.Increment(ref this.sendCounter) % 5 > 0;
            var status = delivered
                ? NotificationStatus.Delivered
                : NotificationStatus.NotDelivered;
            this.statusRepository.SaveStatus(notificationId, status);
            return new(notificationId, delivered);
        }
    }
}