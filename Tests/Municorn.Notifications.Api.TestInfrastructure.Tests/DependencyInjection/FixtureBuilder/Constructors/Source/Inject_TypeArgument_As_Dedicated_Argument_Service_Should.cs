using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureBuilder;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureBuilder.Constructors.Source
{
    [TestFixtureSourceInjectable(typeof(StandardSource))]
    [PrimaryConstructor]
    internal partial class Inject_TypeArgument_As_Dedicated_Argument_Service_Should<T>
    {
        private readonly string argument;
        private readonly ITest service;

        [Test]
        public void Case()
        {
            typeof(T).Should().Be(typeof(int));
            this.argument.Should().Be("passed");
            this.service.Should().NotBeNull();
        }
    }
}
