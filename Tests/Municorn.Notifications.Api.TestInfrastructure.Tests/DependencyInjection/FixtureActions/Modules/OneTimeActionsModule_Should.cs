using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureActions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FixtureOneTimeActions;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureActions.Modules
{
    [TestFixture]
    internal class OneTimeActionsModule_Should : IFixtureWithServiceProviderFramework, IOneTimeSetUpAction
    {
        private readonly Counter counter = new();

        public void ConfigureServices(IServiceCollection serviceCollection) =>
            serviceCollection.AddFixtureOneTimeActions();

        public void OneTimeSetUp() => this.counter.Increment();

        [Test]
        [Repeat(2)]
        public void Case() => this.counter.Value.Should().Be(1);
    }
}
