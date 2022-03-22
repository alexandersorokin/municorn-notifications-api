using System;

namespace Municorn.Notifications.Api
{
    public record SendResponse(Guid Id, SendStatus Status);
}
