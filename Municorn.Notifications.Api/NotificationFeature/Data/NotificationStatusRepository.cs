using System;
using System.Collections.Concurrent;
using JetBrains.Annotations;
using Kontur.Results;

namespace Municorn.Notifications.Api.NotificationFeature.Data
{
    public class NotificationStatusRepository
    {
        private readonly ConcurrentDictionary<Guid, NotificationStatus> statuses = new();

        public void SaveStatus(Guid notificationId, NotificationStatus status)
        {
            this.statuses[notificationId] = status;
        }

        [MustUseReturnValue]
        public Optional<NotificationStatus> GetStatus(Guid notificationId)
        {
            return this.statuses.TryGetValue(notificationId, out var status)
                ? status
                : Optional.None();
        }
    }
}
