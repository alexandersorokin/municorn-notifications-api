using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureBuilder;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.BeforeFixtureConstructor.Constructors
{
    [TestFixtureInjectable(TypeArgs = new[] { typeof(int) })]
    [PrimaryConstructor]
    internal partial class Inject_TypeArgument_As_Dedicated_Should<T>
    {
        [Test]
        public void Case()
        {
            typeof(T).Should().Be(typeof(int));
        }
    }
}
