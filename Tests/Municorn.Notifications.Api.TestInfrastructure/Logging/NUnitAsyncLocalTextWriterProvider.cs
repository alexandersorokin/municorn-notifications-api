using System.IO;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Logging
{
    public sealed class NUnitAsyncLocalTextWriterProvider : ITextWriterProvider
    {
        public TextWriter GetWriter() => TestContext.Progress;
    }
}