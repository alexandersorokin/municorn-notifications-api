using System.IO;

namespace Municorn.Notifications.Api.Tests.Logging
{
    internal interface ITextWriterProvider
    {
        TextWriter Get();
    }
}
