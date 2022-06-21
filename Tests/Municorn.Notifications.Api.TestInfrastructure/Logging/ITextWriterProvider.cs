using System.IO;

namespace Municorn.Notifications.Api.TestInfrastructure.Logging
{
    public interface ITextWriterProvider
    {
        TextWriter GetWriter();
    }
}
