using System.Threading.Tasks;

namespace Municorn.Notifications.Api.NotificationFeature.App
{
    public interface INotification
    {
        Task<SendResult> Send();
    }
}
