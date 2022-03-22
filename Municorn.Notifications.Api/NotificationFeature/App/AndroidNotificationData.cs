using JetBrains.Annotations;

namespace Municorn.Notifications.Api.NotificationFeature.App
{
    public sealed record AndroidNotificationData(
        string DeviceToken,
        string Message,
        string Title) : INotificationData
    {
        public string? Condition { get; init; }

        [MustUseReturnValue]
        public string SerializeToString() => this.ToString();
    }
}
