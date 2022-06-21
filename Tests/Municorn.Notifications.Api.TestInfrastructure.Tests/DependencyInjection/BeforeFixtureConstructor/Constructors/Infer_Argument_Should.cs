using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.BeforeFixtureConstructor;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.BeforeFixtureConstructor.Constructors
{
    [TestFixtureInjectable("passed")]
    [PrimaryConstructor]
    internal partial class Infer_Argument_Should<T>
    {
        private readonly string argument;

        [Test]
        public void Case()
        {
            typeof(T).Should().Be(typeof(string));
            this.argument.Should().Be("passed");
        }
    }
}
