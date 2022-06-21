using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.Tests.DependencyInjection.ScopeMethodInject;
using Municorn.Notifications.Api.Tests.NUnitAttributes;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.AfterFixtureConstructor.Tests
{
    [TestFixture]
    internal class Inject_Method_CombinatorialTestCaseSource_Should : IConfigureServices
    {
        public void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection
            .AddScoped<ILog, SilentLog>();

        [CombinatorialTestCaseSource(nameof(CaseValues))]
        [Repeat(2)]
        public void Cases(int value, [Inject] ILog service)
        {
            service.Should().NotBeNull();
        }

        private static readonly TestCaseData[] CaseValues =
        {
            new(10),
            new(11),
        };
    }
}
