using System.IO;
using Municorn.Notifications.Api.Tests.Log;

namespace Municorn.Notifications.Api.Tests.ApiTests
{
    [PrimaryConstructor]
    internal partial class AdHocTextWriterProvider : ITextWriterProvider
    {
        private readonly TextWriter textWriter;

        public TextWriter Get() => this.textWriter;
    }
}