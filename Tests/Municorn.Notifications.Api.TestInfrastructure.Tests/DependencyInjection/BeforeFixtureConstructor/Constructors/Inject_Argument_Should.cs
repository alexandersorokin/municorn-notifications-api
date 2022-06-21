using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.BeforeFixtureConstructor;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.BeforeFixtureConstructor.Constructors
{
    [TestFixtureInjectable("passed")]
    internal class Inject_Argument_Should
    {
        private readonly string argument;

        public Inject_Argument_Should(string argument) => this.argument = argument;

        [Test]
        public void Case()
        {
            this.argument.Should().Be("passed");
        }
    }
}
