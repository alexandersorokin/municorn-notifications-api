using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Abstractions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FixtureOneTimeActions;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Modules.FixtureOneTimeActions
{
    [FixtureOneTimeActionsModule]
    internal class Attribute_Should : FrameworkServiceProviderFixtureBase, IOneTimeSetUpAction
    {
        private readonly Counter counter = new();

        public Attribute_Should()
            : base(serviceCollection => serviceCollection
                .AddFixtureServiceCollectionModuleAttributes(typeof(Attribute_Should)))
        {
        }

        public void OneTimeSetUp() => this.counter.Increment();

        [Test]
        public void Plain_Test() => this.counter.Value.Should().Be(1);
    }
}
