using System;

namespace Municorn.Notifications.Api.NotificationFeature.App
{
    public record SendResult(Guid Id, bool Delivered);
}
