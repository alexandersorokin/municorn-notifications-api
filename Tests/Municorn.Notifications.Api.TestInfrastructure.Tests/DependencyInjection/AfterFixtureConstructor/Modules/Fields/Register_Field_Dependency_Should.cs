using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Fields;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.AfterFixtureConstructor.Modules.Fields
{
    [TestFixture]
    [FieldDependenciesModule]
    internal class Register_Field_Dependency_Should : IWithoutConfigureServices
    {
        [FieldDependency]
        [RegisterDependency]
        private readonly SilentLog service = default!;

        [Test]
        [Repeat(2)]
        public void Case()
        {
            this.service.Should().NotBeNull();
        }

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(2)]
        public void Cases(int value)
        {
            this.service.Should().NotBeNull();
        }
    }
}
