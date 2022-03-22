using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Kontur.Results;
using Microsoft.AspNetCore.Mvc;

namespace Municorn.Notifications.Api
{
    public interface INotificationsClient
    {
        [MustUseReturnValue]
        public Task<Result<ProblemDetails, SendResponse>> SendNotificationAsync(SendNotificationRequest notification);

        [MustUseReturnValue]
        public Task<Result<ProblemDetails, SendStatusResponse>> GetNotificationStatusAsync(Guid notificationId);
    }
}
