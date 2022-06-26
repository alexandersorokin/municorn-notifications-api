using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureActions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureActions.Modules.Attributes
{
    [TestFixture]
    [FieldInjectionWithFixtureOneTimeActionsModule]
    internal class Fields_With_OneTimeActions_Module_Should : IFixtureWithServiceProviderFramework
    {
        [FieldDependency]
        private readonly SilentLog service = default!;

        public void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection
            .AddSingleton<SilentLog>();

        [Test]
        [Repeat(2)]
        public void Case() => this.service.Should().NotBeNull();
    }
}
