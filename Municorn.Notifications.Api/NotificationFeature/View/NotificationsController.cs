using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Municorn.Notifications.Api.Infrastructure;
using Municorn.Notifications.Api.NotificationFeature.Data;

namespace Municorn.Notifications.Api.NotificationFeature.View
{
    [Route(NotificationsApiUris.Notifications)]
    public class NotificationsController : ControllerBase
    {
        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesProblemDetailsResponse(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SendResponse>> Send(
            SendNotificationRequest request,
            [FromServices] INotificationRequestConverter notificationRequestConverter)
        {
            var notification = request.Accept(notificationRequestConverter);
            var (id, delivered) = await notification.Send().ConfigureAwait(false);
            var status = delivered ? SendStatus.Delivered : SendStatus.NotDelivered;
            var location = new Uri(
                $"{this.HttpContext.Request.GetEncodedUrl()}/{id}/status",
                UriKind.Absolute);
            return this.Created(location, new SendResponse(id, status));
        }

        [HttpGet]
        [Route("{notificationId:guid}/status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesProblemDetailsResponse(StatusCodes.Status400BadRequest)]
        [ProducesProblemDetailsResponse(StatusCodes.Status404NotFound)]
        public ActionResult<SendStatusResponse> GetStatus(
            Guid notificationId,
            [FromServices] NotificationStatusRepository notificationStatusRepository)
        {
            var statusState = notificationStatusRepository.GetStatus(notificationId);
            if (!statusState.TryGetValue(out var notificationStatus))
            {
                return this.NotFound();
            }

            var status = notificationStatus == NotificationStatus.Delivered
                ? SendStatus.Delivered
                : SendStatus.NotDelivered;
            return new SendStatusResponse(status);
        }
    }
}