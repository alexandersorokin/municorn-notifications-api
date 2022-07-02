using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureBuilder;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.TestCommunication;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureBuilder.Modules.FoAll.Communication
{
    [TestFixtureInjectable]
    [TestCommunicationModule]
    [MockServiceModule]
    [TestActionLoggerSuite]
    [TestActionLoggerTest]
    internal class Pass_Container_To_TestAction_Attribute_Should
    {
        [Test]
        [Repeat(2)]
        public void Case() => true.Should().BeTrue();

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(2)]
        public void Cases(int value) => value.Should().BePositive();
    }
}
