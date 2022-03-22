using System;
using Microsoft.Extensions.Logging;

namespace Municorn.Notifications.Api.Infrastructure
{
    public record HostSettings(
        Uri Uri,
        Action<ILoggingBuilder> ConfigureLogging);
}
