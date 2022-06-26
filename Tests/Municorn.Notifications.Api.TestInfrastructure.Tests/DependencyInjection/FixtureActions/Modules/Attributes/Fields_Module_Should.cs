using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureActions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureActions.Modules.Attributes
{
    [TestFixture]
    [FieldInjectionModule]
    internal class Fields_Module_Should : IFixtureWithServiceProviderFramework
    {
        [FieldDependency]
        private readonly MockService service = default!;

        public void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection
            .AddSingleton<MockService>();

        [Test]
        [Repeat(2)]
        public void Case() => this.service.Should().NotBeNull();
    }
}
