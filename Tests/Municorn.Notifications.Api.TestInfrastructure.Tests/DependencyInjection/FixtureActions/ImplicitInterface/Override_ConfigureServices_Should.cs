using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureActions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureActions.ImplicitInterface
{
    [TestFixture]
    internal class Override_ConfigureServices_Should : IWithNoServices
    {
        [FieldDependency]
        private readonly ILog service = default!;

        public void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection
            .AddFieldInjection(this)
            .AddSingleton<ILog, SilentLog>();

        [Test]
        [Repeat(2)]
        public void Case() => this.service.Should().NotBeNull();
    }
}
