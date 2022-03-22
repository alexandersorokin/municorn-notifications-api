using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.Tests.ApiTests
{
    [SetUpFixture]
    internal class GlobalLog
    {
        public ILog BoundLog { get; } = NUnitLog.CreateBoundToCurrentContext();
    }
}
