namespace Municorn.Notifications.Api
{
    public interface INotificationRequestVisitor<out TResult>
    {
        public TResult Visit(IosSendNotificationRequest request);

        public TResult Visit(AndroidSendNotificationRequest request);
    }
}
