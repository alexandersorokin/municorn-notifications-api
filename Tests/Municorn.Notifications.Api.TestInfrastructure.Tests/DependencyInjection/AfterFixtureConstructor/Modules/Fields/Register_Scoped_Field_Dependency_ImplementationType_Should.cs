using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.TestCommunication;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.TestCommunication.AsyncLocal;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.AfterFixtureConstructor.Modules.Fields
{
    [TestFixture]
    [FieldInjectionModule]
    [TestCommunicationModule]
    internal class Register_Scoped_Field_Dependency_ImplementationType_Should : IWithoutConfigureServices
    {
        [FieldDependency]
        [RegisterDependency(typeof(SilentLog))]
        private readonly IAsyncLocalServiceProvider<ILog> service = default!;

        [Test]
        [Repeat(2)]
        public void Case()
        {
            this.service.Value.Should().NotBeNull();
        }

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(2)]
        public void Cases(int value)
        {
            this.service.Value.Should().NotBeNull();
        }
    }
}
