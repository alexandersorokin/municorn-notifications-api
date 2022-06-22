using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.BeforeFixtureConstructor;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Scopes.AsyncLocal;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.BeforeFixtureConstructor.Constructors
{
    [TestFixtureInjectable(TypeArgs = new[] { typeof(int) })]
    [PrimaryConstructor]
    internal partial class Inject_TypeArgument_As_Argument_Service_Should<T>
    {
        private readonly AsyncLocalServiceProvider service;

        [Test]
        public void Case()
        {
            typeof(T).Should().Be(typeof(int));
            this.service.Should().NotBeNull();
        }
    }
}
