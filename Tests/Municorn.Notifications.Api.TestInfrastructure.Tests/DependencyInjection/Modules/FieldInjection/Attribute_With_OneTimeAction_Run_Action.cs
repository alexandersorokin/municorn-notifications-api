using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Abstractions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FixtureOneTimeActions;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Modules.FieldInjection
{
    [FieldInjectionWithFixtureOneTimeActionsModule]
    internal class Attribute_With_OneTimeAction_Run_Action : FrameworkServiceProviderFixtureBase, IOneTimeSetUpAction
    {
        private readonly Counter counter = new();

        public Attribute_With_OneTimeAction_Run_Action()
            : base(serviceCollection => serviceCollection
                .AddFixtureServiceCollectionModuleAttributes(typeof(Attribute_With_OneTimeAction_Run_Action)))
        {
        }

        public void OneTimeSetUp() => this.counter.Increment();

        [Test]
        public void Case() => this.counter.Value.Should().Be(1);
    }
}
