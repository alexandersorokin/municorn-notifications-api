using JetBrains.Annotations;

namespace Municorn.Notifications.Api.NotificationFeature.App
{
    public sealed record IosNotificationData(
        string PushToken,
        string Alert) : INotificationData
    {
        public int? Priority { get; init; }

        public bool? IsBackground { get; init; }

        [MustUseReturnValue]
        public string SerializeToString() => this.ToString();
    }
}
