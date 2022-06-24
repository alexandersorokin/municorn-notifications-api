using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.BeforeFixtureConstructor;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Communication;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Communication.AsyncLocal;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.BeforeFixtureConstructor.Communication
{
    [TestFixtureInjectable]
    [TestCommunicationModule]
    [TimeLoggerModule]
    [PrimaryConstructor]
    internal partial class Inject_AsyncLocal_Should
    {
        private readonly IAsyncLocalServiceProvider<IFixtureSetUp> service;

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
