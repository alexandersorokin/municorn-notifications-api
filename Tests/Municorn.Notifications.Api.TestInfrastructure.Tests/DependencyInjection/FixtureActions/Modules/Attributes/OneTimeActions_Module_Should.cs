using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FixtureOneTimeActions;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureActions.Modules.Attributes
{
    [TestFixture]
    [FixtureOneTimeActionsModule]
    internal class OneTimeActions_Module_Should : IWithoutServices, IOneTimeSetUpAction
    {
        private readonly Counter counter = new();

        public void OneTimeSetUp() => this.counter.Increment();

        [Test]
        [Repeat(2)]
        public void Case() => this.counter.Value.Should().Be(1);
    }
}
