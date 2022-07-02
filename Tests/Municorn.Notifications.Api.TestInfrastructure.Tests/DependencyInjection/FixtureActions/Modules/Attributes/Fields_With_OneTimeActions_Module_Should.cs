using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Combo;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FixtureOneTimeActions;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureActions.Modules.Attributes
{
    [TestFixture]
    [FieldInjectionWithFixtureOneTimeActionsModule]
    internal class Fields_With_OneTimeActions_Module_Should : IWithoutServices, IOneTimeSetUpAction
    {
        private readonly Counter service = new();

        public void OneTimeSetUp() => this.service.Increment();

        [Test]
        [Repeat(2)]
        public void Case() => this.service.Value.Should().Be(1);
    }
}
