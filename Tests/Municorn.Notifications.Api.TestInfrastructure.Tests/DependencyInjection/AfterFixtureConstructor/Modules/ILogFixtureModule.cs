using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.AfterFixtureConstructor.Modules
{
    [FixtureModule(typeof(ILog), typeof(SilentLog))]
    internal interface ILogFixtureModule
    {
    }
}
