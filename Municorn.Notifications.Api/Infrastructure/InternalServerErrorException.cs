using System;

namespace Municorn.Notifications.Api.Infrastructure
{
    public class InternalServerErrorException : Exception
    {
        internal InternalServerErrorException(string message)
            : base(message)
        {
        }
    }
}
