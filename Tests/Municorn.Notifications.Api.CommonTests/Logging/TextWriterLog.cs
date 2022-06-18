using Vostok.Logging.Abstractions;
using Vostok.Logging.Formatting;

namespace Municorn.Notifications.Api.Tests.Logging
{
    internal class TextWriterLog : ILog
    {
        private static readonly OutputTemplate Template = new OutputTemplateBuilder()
            .AddTimestamp()
            .AddText(" ")
            .AddLevel()
            .AddText(" ")
            .AddMessage()
            .AddNewline()
            .AddException()
            .Build();

        private readonly ITextWriterProvider textWriterProvider;

        public TextWriterLog(ITextWriterProvider textWriterProvider)
        {
            this.textWriterProvider = textWriterProvider;
        }

        public void Log(LogEvent? @event)
        {
            if (@event is null)
            {
                return;
            }

            var message = LogEventFormatter.Format(@event, Template);
            this.textWriterProvider.Get().Write(message);
        }

        public bool IsEnabledFor(LogLevel level) => true;

        public ILog ForContext(string context) => this;
    }
}
