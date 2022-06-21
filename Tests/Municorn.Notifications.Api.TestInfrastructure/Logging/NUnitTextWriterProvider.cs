using System.IO;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Logging
{
    public class NUnitTextWriterProvider : ITextWriterProvider
    {
        public TextWriter GetWriter() => TestContext.Progress;
    }
}