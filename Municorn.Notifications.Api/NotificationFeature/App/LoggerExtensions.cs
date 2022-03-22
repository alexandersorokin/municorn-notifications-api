using System;
using Microsoft.Extensions.Logging;

namespace Municorn.Notifications.Api.NotificationFeature.App
{
    internal static class LoggerExtensions
    {
        private static readonly Action<ILogger, string, string, Exception?> SendNotificationSentLogMessage = LoggerMessage.Define<string, string>(
            LogLevel.Information,
            new(1),
            "{Sender} sent {Notification}");

        internal static void NotificationSent<TNotificationData>(
            this ILogger logger,
            string sender,
            TNotificationData notification)
            where TNotificationData : INotificationData
        {
            SendNotificationSentLogMessage(logger, sender, notification.SerializeToString(), null);
        }
    }
}
