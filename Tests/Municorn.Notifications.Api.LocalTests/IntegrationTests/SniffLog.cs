using Vostok.Logging.Abstractions;
using Vostok.Logging.Formatting;

namespace Municorn.Notifications.Api.Tests.IntegrationTests
{
    [PrimaryConstructor]
    internal partial class SniffLog : ILog
    {
        private static readonly OutputTemplate Template = new OutputTemplateBuilder()
            .AddMessage()
            .Build();

        private readonly LogMessageContainer container;

        public void Log(LogEvent @event)
        {
            var message = LogEventFormatter.Format(@event, Template);
            this.container.Add(message);
        }

        public bool IsEnabledFor(LogLevel level) => true;

        public ILog ForContext(string context) => this;
    }
}
