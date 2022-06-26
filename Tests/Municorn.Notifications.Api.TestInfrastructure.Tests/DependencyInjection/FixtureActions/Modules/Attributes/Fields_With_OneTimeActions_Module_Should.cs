using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureActions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FixtureOneTimeActions;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureActions.Modules.Attributes
{
    [TestFixture]
    [FieldInjectionWithFixtureOneTimeActionsModule]
    internal class Fields_With_OneTimeActions_Module_Should : IFixtureWithServiceProviderFramework, IOneTimeSetUpAction
    {
        [FieldDependency]
        private readonly Counter service = default!;

        public void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection
            .AddSingleton<Counter>();

        public void OneTimeSetUp() => this.service.Increment();

        [Test]
        [Repeat(2)]
        public void Case() => this.service.Value.Should().Be(1);
    }
}
