using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureActions.ImplicitInterface
{
    [TestFixture]
    internal class Do_Not_Require_To_Implement_ConfigureServices_Should : IWithFieldInjectionServices
    {
        [FieldDependency]
        private readonly ITest service = default!;

        [Test]
        [Repeat(2)]
        public void Case() => this.service.Should().NotBeNull();
    }
}
