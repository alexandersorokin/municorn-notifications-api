using System.IO;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Logging
{
    public class NUnitAsyncLocalTextWriterProvider : ITextWriterProvider
    {
        public TextWriter GetWriter() => TestContext.Progress;
    }
}