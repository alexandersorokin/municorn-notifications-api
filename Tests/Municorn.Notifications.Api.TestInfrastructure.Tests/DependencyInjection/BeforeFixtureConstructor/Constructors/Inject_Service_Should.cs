using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.BeforeFixtureConstructor;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.ScopeAsyncLocal;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.BeforeFixtureConstructor.Constructors
{
    [TestFixtureInjectable]
    internal class Inject_Service_Should
    {
        private readonly AsyncLocalTestCaseServiceResolver service;

        public Inject_Service_Should(AsyncLocalTestCaseServiceResolver service) => this.service = service;

        [Test]
        public void Case()
        {
            this.service.Should().NotBeNull();
        }
    }
}
