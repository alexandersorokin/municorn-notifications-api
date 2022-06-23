using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.BeforeFixtureConstructor;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.BeforeFixtureConstructor.Constructors
{
    [TestFixtureInjectable]
    [PrimaryConstructor]
    internal partial class Inject_Service_Should
    {
        private readonly ITest service;

        [Test]
        public void Case()
        {
            this.service.Should().NotBeNull();
        }
    }
}
