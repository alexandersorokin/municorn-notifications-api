namespace Municorn.Notifications.Api
{
    public abstract record SendNotificationRequest
    {
        public abstract TResult Accept<TResult>(INotificationRequestVisitor<TResult> requestVisitor);
    }
}
