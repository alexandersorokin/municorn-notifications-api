using System.IO;

namespace Municorn.Notifications.Api.Tests.Logging
{
    [PrimaryConstructor]
    internal partial class AdHocTextWriterProvider : ITextWriterProvider
    {
        private readonly TextWriter textWriter;

        public TextWriter GetWriter() => this.textWriter;
    }
}