using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureBuilder;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureBuilder.Modules.ForFixtureBuilderOnly.ModuleService
{
    [TestFixtureInjectable]
    [TestMethodInjectionModule]
    [FixtureModuleService(typeof(IMockService), typeof(MockService))]
    internal class Use_Module_Service_Implementation_Should
    {
        [Test]
        [Repeat(2)]
        public void Case([InjectDependency] IMockService service) => service.Should().NotBeNull();

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(2)]
        public void Cases([InjectDependency] IMockService service, int value) => service.Should().NotBeNull();
    }
}
