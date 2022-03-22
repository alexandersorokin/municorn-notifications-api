using System;
using System.IO;
using NUnit.Framework;
using Vostok.Logging.Abstractions;
using Vostok.Logging.Formatting;

namespace Municorn.Notifications.Api.Tests
{
    internal class NUnitLog : ILog
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

        private readonly Func<TextWriter> writerFactory;

        private NUnitLog(Func<TextWriter> writerFactory)
        {
            this.writerFactory = writerFactory;
        }

        public void Log(LogEvent? @event)
        {
            if (@event is null)
            {
                return;
            }

            var message = LogEventFormatter.Format(@event, Template);
            this.writerFactory().Write(message);
        }

        public bool IsEnabledFor(LogLevel level) => true;

        public ILog ForContext(string context) => this;

        internal static NUnitLog CreateContextual()
        {
            return new(() => TestContext.Progress);
        }

        internal static NUnitLog CreateBoundToCurrentContext()
        {
            var textWriter = TestContext.Out;
            return new(() => textWriter);
        }
    }
}
