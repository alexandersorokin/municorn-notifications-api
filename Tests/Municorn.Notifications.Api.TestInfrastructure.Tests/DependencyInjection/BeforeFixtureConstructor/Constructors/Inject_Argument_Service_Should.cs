using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.BeforeFixtureConstructor;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Scopes.AsyncLocal;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.BeforeFixtureConstructor.Constructors
{
    [TestFixtureInjectable("passed")]
    [PrimaryConstructor]
    internal partial class Inject_Argument_Service_Should
    {
        private readonly string argument;
        private readonly AsyncLocalServiceProvider service;

        [Test]
        public void Case()
        {
            this.argument.Should().Be("passed");
            this.service.Should().NotBeNull();
        }
    }
}
