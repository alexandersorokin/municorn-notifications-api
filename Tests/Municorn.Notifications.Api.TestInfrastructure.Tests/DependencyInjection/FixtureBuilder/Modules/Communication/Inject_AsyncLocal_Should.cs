using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureBuilder;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.TestCommunication;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureBuilder.Modules.Communication
{
    [TestFixtureInjectable]
    [TestCommunicationModule]
    [MockServiceScopedModule]
    [PrimaryConstructor]
    internal partial class Inject_AsyncLocal_Should
    {
        private readonly IAsyncLocalServiceProvider<MockService> service;

        [Test]
        [Repeat(2)]
        public void Case() => this.service.Value.Should().NotBeNull();

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(2)]
        public void Cases(int value) => this.service.Value.Should().NotBeNull();
    }
}
