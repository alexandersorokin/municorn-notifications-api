using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.AfterFixtureConstructor.Modules
{
    [FixtureModuleService(typeof(ILog), typeof(SilentLog))]
    internal interface ILogFixtureModule
    {
    }
}
