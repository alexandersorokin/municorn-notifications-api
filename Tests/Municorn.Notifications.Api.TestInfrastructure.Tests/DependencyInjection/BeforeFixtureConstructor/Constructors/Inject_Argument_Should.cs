using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureBuilder;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.BeforeFixtureConstructor.Constructors
{
    [TestFixtureInjectable("passed")]
    [PrimaryConstructor]
    internal partial class Inject_Argument_Should
    {
        private readonly string argument;

        [Test]
        public void Case()
        {
            this.argument.Should().Be("passed");
        }
    }
}
