namespace Municorn.Notifications.Api
{
    [Discriminator("ios")]
    public sealed record IosSendNotificationRequest(
        string PushToken,
        string Alert) : SendNotificationRequest
    {
        public int? Priority { get; init; } = 10;

        public bool? IsBackground { get; init; } = true;

        public override TResult Accept<TResult>(INotificationRequestVisitor<TResult> requestVisitor) => requestVisitor.Visit(this);
    }
}
