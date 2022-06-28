using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureBuilder;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureBuilder.Modules.Scoped
{
    [TestFixtureInjectable]
    [TestMethodInjectionModule]
    [RegisterScopedInterfaceModule]
    internal class Use_Scoped_Interface_Should : IRegisterScoped<MockService>
    {
        public MockService Get() => new();

        [Test]
        [Repeat(2)]
        public void Case([InjectDependency] MockService service) => service.Should().NotBeNull();

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(2)]
        public void Cases([InjectDependency] MockService service, int value) => service.Should().NotBeNull();
    }
}
