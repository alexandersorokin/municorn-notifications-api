using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor.Fields;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.AfterFixtureConstructor
{
    [TestFixture]
    internal class Inject_Test_Should : IWithNoServices
    {
        [TestDependency]
        private readonly ITest service = default!;

        [Test]
        [Repeat(2)]
        public void Case()
        {
            this.service.Should().NotBeNull();
        }

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(2)]
        public void Cases(int value)
        {
            this.service.Should().NotBeNull();
        }
    }
}
