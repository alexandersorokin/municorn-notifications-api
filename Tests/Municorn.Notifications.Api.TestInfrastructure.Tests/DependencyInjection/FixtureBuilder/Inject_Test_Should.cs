using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureBuilder;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureBuilder
{
    [TestFixtureInjectable]
    [PrimaryConstructor]
    internal partial class Inject_Test_Should
    {
        private readonly ITest service;

        [Test]
        [Repeat(2)]
        public void Case() => this.service.Should().NotBeNull();

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(2)]
        public void Cases(int value) => this.service.Should().NotBeNull();
    }
}
