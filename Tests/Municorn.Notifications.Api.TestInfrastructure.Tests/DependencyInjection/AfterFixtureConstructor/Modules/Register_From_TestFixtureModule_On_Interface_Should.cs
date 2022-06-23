using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.AfterFixtureConstructor.Modules
{
    [TestFixture]
    [TestMethodInjectionModule]
    internal class Register_From_TestFixtureModule_On_Interface_Should : IWithoutConfigureServices, ILogFixtureModule
    {
        [Test]
        [Repeat(2)]
        public void Case([InjectDependency] ILog service)
        {
            service.Should().NotBeNull();
        }
    }
}
