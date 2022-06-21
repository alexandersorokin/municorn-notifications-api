using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.BeforeFixtureConstructor;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.ScopeAsyncLocal;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.BeforeFixtureConstructor.Source
{
    [TestFixtureSourceInjectable(typeof(Inject_Argument_Service_Should), nameof(Inject_Argument_Service_Should.FixtureData))]
    [PrimaryConstructor]
    internal partial class Inject_Argument_Service_From_Other_Class_Should
    {
        private readonly string argument;
        private readonly AsyncLocalTestCaseServiceResolver service;

        [Test]
        public void Case()
        {
            this.argument.Should().Be("passed");
            this.service.Should().NotBeNull();
        }
    }
}
