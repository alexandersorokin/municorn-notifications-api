using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureActions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.TestCommunication;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureActions.Modules.Communication
{
    [TestFixture]
    [TestActionLoggerTest]
    [TestActionLoggerSuite]
    internal class Pass_Container_To_Attribute_Should : IFixtureWithServiceProviderFramework
    {
        public void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection
            .AddTestCommunication()
            .AddScoped<MockService>();

        [Test]
        [Repeat(2)]
        public void Case() => true.Should().BeTrue();

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(2)]
        public void Cases(int value) => value.Should().BePositive();
    }
}
