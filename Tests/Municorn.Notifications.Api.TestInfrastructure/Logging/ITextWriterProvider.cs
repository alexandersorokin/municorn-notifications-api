using System.IO;

namespace Municorn.Notifications.Api.Tests.Logging
{
    public interface ITextWriterProvider
    {
        TextWriter GetWriter();
    }
}
