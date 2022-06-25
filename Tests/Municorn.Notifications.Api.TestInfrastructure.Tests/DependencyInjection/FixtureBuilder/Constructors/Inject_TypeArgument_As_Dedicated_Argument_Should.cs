using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureBuilder;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureBuilder.Constructors
{
    [TestFixtureInjectable(typeof(int), "passed")]
    [PrimaryConstructor]
    internal partial class Inject_TypeArgument_As_Dedicated_Argument_Should<T>
    {
        private readonly string argument;

        [Test]
        public void Case()
        {
            typeof(T).Should().Be(typeof(int));
            this.argument.Should().Be("passed");
        }
    }
}
