using System.Threading.Tasks;

namespace Municorn.Notifications.Api.NotificationFeature.App
{
    public interface INotificationSender<in TData>
    {
        Task<SendResult> Send(TData request);
    }
}
