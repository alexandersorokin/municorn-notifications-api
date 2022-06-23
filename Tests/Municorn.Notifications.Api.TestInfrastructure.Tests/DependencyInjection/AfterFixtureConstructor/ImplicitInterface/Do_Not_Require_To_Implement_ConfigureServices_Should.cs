using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Fields;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Scopes.AsyncLocal;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.AfterFixtureConstructor.ImplicitInterface
{
    [TestFixture]
    [FieldDependenciesModule]
    internal class Do_Not_Require_To_Implement_ConfigureServices_Should : IWithNoServices
    {
        [FieldDependency]
        private readonly AsyncLocalServiceProvider service = default!;

        [Test]
        [Repeat(2)]
        public void Case()
        {
            this.service.Should().NotBeNull();
        }
    }
}
