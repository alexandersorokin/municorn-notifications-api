using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureBuilder;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection;
using Municorn.Notifications.Api.TestInfrastructure.Logging;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.BeforeFixtureConstructor.Modules.Scoped
{
    [TestFixtureInjectable]
    [ScopedInterfaceModule]
    [TestMethodInjectionModule]
    internal class Use_Scoped_Interface_Should : IScoped<ILog>
    {
        public ILog Get() => new TextWriterLog(new NUnitAsyncLocalTextWriterProvider());

        [Test]
        [Repeat(2)]
        public void Case([InjectDependency] ILog service)
        {
            service.Should().NotBeNull();
        }

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(2)]
        public void Cases([InjectDependency] ILog service, int value)
        {
            service.Should().NotBeNull();
        }
    }
}
