using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureBuilder;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureBuilder.Constructors.Source
{
    [TestFixtureSourceInjectable(typeof(Inject_Argument_Service_Should), nameof(Inject_Argument_Service_Should.FixtureData))]
    [PrimaryConstructor]
    internal partial class Inject_Argument_Service_From_Other_Class_Should
    {
        private readonly string argument;
        private readonly ITest service;

        [Test]
        public void Case()
        {
            this.argument.Should().Be("passed");
            this.service.Should().NotBeNull();
        }
    }
}
