using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.ScopeAsyncLocal;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.AfterFixtureConstructor.ImplicitInterface
{
    [TestFixture]
    internal class Do_Not_Require_To_Implement_ConfigureServices_Should : IWithDependencyInjection
    {
        [TestDependency]
        private readonly AsyncLocalTestCaseServiceResolver service = default!;

        [Test]
        [Repeat(2)]
        public void Case()
        {
            this.service.Should().NotBeNull();
        }
    }
}
