using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.BeforeFixtureConstructor;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Scopes.AsyncLocal;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.BeforeFixtureConstructor.Constructors
{
    [TestFixtureInjectable]
    [PrimaryConstructor]
    internal partial class Inject_Service_Should
    {
        private readonly AsyncLocalServiceProvider service;

        [Test]
        public void Case()
        {
            this.service.Should().NotBeNull();
        }
    }
}
