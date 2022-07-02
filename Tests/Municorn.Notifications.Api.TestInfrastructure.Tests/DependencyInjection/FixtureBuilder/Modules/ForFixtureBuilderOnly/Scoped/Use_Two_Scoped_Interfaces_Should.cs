using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureBuilder;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureBuilder.Modules.ForFixtureBuilderOnly.Scoped
{
    [TestFixtureInjectable]
    [TestMethodInjectionModule]
    [RegisterScopedInterfaceModule]
    internal class Use_Two_Scoped_Interfaces_Should : IRegisterScoped<IMockService>, IRegisterScoped<MockService>
    {
        IMockService IRegisterScoped<IMockService>.Get() => new MockService();

        MockService IRegisterScoped<MockService>.Get() => new();

        [Test]
        [Repeat(2)]
        public void Case([InjectParameterDependency] IMockService service1, [InjectParameterDependency] MockService service2)
        {
            service1.Should().NotBeNull();
            service2.Should().NotBeNull();
        }

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(2)]
        public void Cases([InjectParameterDependency] IMockService service1, int value, [InjectParameterDependency] MockService service2)
        {
            service1.Should().NotBeNull();
            service2.Should().NotBeNull();
        }
    }
}
