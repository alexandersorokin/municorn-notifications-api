using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.BeforeFixtureConstructor;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Communicat1ion;
using Municorn.Notifications.Api.TestInfrastructure.Logging;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.BeforeFixtureConstructor.Modules.Scoped
{
    [TestFixtureInjectable]
    [TestMethodInjectionModule]
    [ScopedInterfaceModule]
    internal class Use_Two_Scoped_Interfaces_Should : IScoped<ILog>, IScoped<Counter>
    {
        ILog IScoped<ILog>.Get() => new TextWriterLog(new NUnitTextWriterProvider());

        Counter IScoped<Counter>.Get() => new();

        [Test]
        [Repeat(2)]
        public void Case([InjectDependency] ILog service1, [InjectDependency] Counter service2)
        {
            service1.Should().NotBeNull();
            service2.Should().NotBeNull();
        }

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(2)]
        public void Cases([InjectDependency] ILog service1, int value, [InjectDependency] Counter service2)
        {
            service1.Should().NotBeNull();
            service2.Should().NotBeNull();
        }
    }
}
