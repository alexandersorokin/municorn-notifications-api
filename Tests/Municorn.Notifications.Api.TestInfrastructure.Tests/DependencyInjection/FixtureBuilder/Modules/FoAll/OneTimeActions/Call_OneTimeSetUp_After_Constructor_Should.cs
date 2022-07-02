using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureBuilder;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FixtureOneTimeActions;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureBuilder.Modules.FoAll.OneTimeActions
{
    [TestFixtureInjectable]
    [FixtureOneTimeActionsModule]
    [MockServiceModule]
    [PrimaryConstructor]
    internal partial class Call_OneTimeSetUp_After_Constructor_Should : IOneTimeSetUpAction
    {
        private readonly MockService service;

        public void OneTimeSetUp() => this.service.Should().NotBeNull();

        [Test]
        [Repeat(2)]
        public void Case() => this.service.Should().NotBeNull();
    }
}
