using System.IO;

namespace Municorn.Notifications.Api.TestInfrastructure.Logging
{
    [PrimaryConstructor]
    public sealed partial class AdHocTextWriterProvider : ITextWriterProvider
    {
        private readonly TextWriter textWriter;

        public TextWriter GetWriter() => this.textWriter;
    }
}