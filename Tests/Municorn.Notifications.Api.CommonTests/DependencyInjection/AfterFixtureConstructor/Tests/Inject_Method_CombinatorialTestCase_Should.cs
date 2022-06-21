using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.Tests.DependencyInjection.ScopeMethodInject;
using Municorn.Notifications.Api.Tests.NUnitAttributes;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.AfterFixtureConstructor.Tests
{
    [TestFixture]
    internal class Inject_Method_CombinatorialTestCase_Should : IConfigureServices
    {
        public void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection
            .AddScoped<ILog, SilentLog>();

        [CombinatorialTestCase]
        [Repeat(2)]
        public void NoParams()
        {
            true.Should().BeTrue();
        }

        [CombinatorialTestCase]
        [Repeat(2)]
        public void Case([Inject] ILog service)
        {
            service.Should().NotBeNull();
        }

        [CombinatorialTestCase(10)]
        [CombinatorialTestCase(11)]
        [Repeat(2)]
        public void Cases(int value, [Inject] ILog service)
        {
            service.Should().NotBeNull();
        }
    }
}
