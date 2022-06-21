using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.BeforeFixtureConstructor;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.ScopeMethodInject;
using Municorn.Notifications.Api.TestInfrastructure.Logging;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.BeforeFixtureConstructor.Scoped
{
    [TestFixtureInjectable]
    [ScopedInterfaceModule]
    [PrimaryConstructor]
    internal partial class Use_Scoped_Interface_Should : IScoped<ILog>
    {
        public ILog Get() => new TextWriterLog(new NUnitTextWriterProvider());

        [Test]
        [Repeat(2)]
        public void Case([Inject] ILog service)
        {
            service.Should().NotBeNull();
        }

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(2)]
        public void Cases([Inject] ILog service, int value)
        {
            service.Should().NotBeNull();
        }
    }
}
