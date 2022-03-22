using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.Tests.IntegrationTests
{
    internal class SniffCompositeLog : CompositeLog
    {
        public SniffCompositeLog(NUnitLog nunitLog, SniffLog sniffLog)
            : base(nunitLog, sniffLog)
        {
        }
    }
}
