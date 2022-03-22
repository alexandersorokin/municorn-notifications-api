namespace Municorn.Notifications.Api
{
    [Discriminator("android")]
    public sealed record AndroidSendNotificationRequest(
        string DeviceToken,
        string Message,
        string Title) : SendNotificationRequest
    {
        public string? Condition { get; init; }

        public override TResult Accept<TResult>(INotificationRequestVisitor<TResult> requestVisitor) => requestVisitor.Visit(this);
    }
}
