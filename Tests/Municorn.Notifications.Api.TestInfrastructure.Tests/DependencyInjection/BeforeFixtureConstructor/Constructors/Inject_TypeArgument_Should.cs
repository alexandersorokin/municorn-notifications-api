using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.BeforeFixtureConstructor;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.BeforeFixtureConstructor.Constructors
{
    [TestFixtureInjectable(TypeArgs = new[] { typeof(int) })]
    [PrimaryConstructor]
    internal partial class Inject_TypeArgument_Should<T>
    {
        [Test]
        public void Case()
        {
            typeof(T).Should().Be(typeof(int));
        }
    }
}
