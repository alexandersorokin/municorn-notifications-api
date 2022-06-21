using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.BeforeFixtureConstructor;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.BeforeFixtureConstructor.Constructors
{
    [TestFixtureInjectable("passed")]
    [TestFixtureInjectable("passed2")]
    [PrimaryConstructor]
    internal partial class Multiple_Arguments_Should
    {
        private readonly string argument;

        [Test]
        public void Case()
        {
            this.argument.Should().NotBeNull();
        }
    }
}
