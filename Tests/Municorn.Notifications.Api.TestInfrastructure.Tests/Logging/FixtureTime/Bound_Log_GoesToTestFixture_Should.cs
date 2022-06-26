using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureActions;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.Logging.FixtureTime
{
    [TestFixture]
    internal class Bound_Log_GoesToTestFixture_Should : IFixtureWithServiceProviderFramework
    {
        public void ConfigureServices(IServiceCollection serviceCollection) =>
            serviceCollection
                .AddBoundLog()
                .AddFixtureTimeLogger();

        [Test]
        public void Test() => true.Should().BeTrue();

        [TestCase(1)]
        [TestCase(2)]
        public void TestCase(int value) => value.Should().BePositive();
    }
}
