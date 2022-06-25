using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Abstractions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FixtureOneTimeActions;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Modules.FieldInjection
{
    [FieldInjectionWithFixtureOneTimeActionsModule]
    internal class Attribute_With_OneTimeAction_Get_Service_Should : FrameworkServiceProviderFixtureBase, IOneTimeSetUpAction
    {
        [FieldDependency]
        private readonly SilentLog service = default!;

        public Attribute_With_OneTimeAction_Get_Service_Should()
            : base(serviceCollection => serviceCollection
                .AddFixtureServiceCollectionModuleAttributes(typeof(Attribute_With_OneTimeAction_Get_Service_Should))
                .AddSingleton<SilentLog>())
        {
        }

        public void OneTimeSetUp() => this.service.Should().NotBeNull();

        [Test]
        public void Case() => this.service.Should().NotBeNull();
    }
}
