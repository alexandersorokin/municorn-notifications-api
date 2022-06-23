using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.BeforeFixtureConstructor;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Scopes.AsyncLocal;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.BeforeFixtureConstructor.Constructors.Source
{
    [TestFixtureSourceInjectable(typeof(StandardSource))]
    [PrimaryConstructor]
    internal partial class Inject_TypeArgument_As_Dedicated_Argument_Service_Should<T>
    {
        private readonly string argument;
        private readonly AsyncLocalServiceProvider service;

        [Test]
        public void Case()
        {
            typeof(T).Should().Be(typeof(int));
            this.argument.Should().Be("passed");
            this.service.Should().NotBeNull();
        }
    }
}
