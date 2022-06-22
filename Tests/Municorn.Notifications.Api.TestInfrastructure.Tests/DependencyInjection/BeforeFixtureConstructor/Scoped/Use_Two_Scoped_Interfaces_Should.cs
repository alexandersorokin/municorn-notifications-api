using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.BeforeFixtureConstructor;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Scopes.Inject;
using Municorn.Notifications.Api.TestInfrastructure.Logging;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.BeforeFixtureConstructor.Scoped
{
    [TestFixtureInjectable]
    [ScopedInterfaceModule]
    internal class Use_Two_Scoped_Interfaces_Should : IScoped<ILog>, IScoped<Counter>
    {
        ILog IScoped<ILog>.Get() => new TextWriterLog(new NUnitTextWriterProvider());

        Counter IScoped<Counter>.Get() => new();

        [Test]
        [Repeat(2)]
        public void Case([Inject] ILog service1, [Inject] Counter service2)
        {
            service1.Should().NotBeNull();
            service2.Should().NotBeNull();
        }

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(2)]
        public void Cases([Inject] ILog service1, int value, [Inject] Counter service2)
        {
            service1.Should().NotBeNull();
            service2.Should().NotBeNull();
        }
    }
}
