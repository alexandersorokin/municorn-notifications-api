using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.ScopeMethodInject;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.AfterFixtureConstructor
{
    [TestFixture]
    internal class Inject_Method_Should : IConfigureServices
    {
        public void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection
            .AddScoped<ILog, SilentLog>();

        [Test]
        [Repeat(2)]
        public void Case([Inject] ILog service)
        {
            service.Should().NotBeNull();
        }

        [Test]
        [Repeat(2)]
        public void Select_Service([Inject(typeof(ILog))] object service)
        {
            service.Should().NotBeNull();
        }

        [Test]
        [Repeat(2)]
        public void Case_With_Provider([Inject] ILog service, [Values] bool value)
        {
            service.Should().NotBeNull();
        }

        [TestCaseSource(nameof(CaseValues))]
        [Repeat(2)]
        public void Cases(int value, ILog service)
        {
            service.Should().NotBeNull();
        }

        private static readonly TestCaseData[] CaseValues =
        {
            CreateCase(10),
            CreateCase(11),
        };

        private static TestCaseData CreateCase(int value) => new(value, new InjectedService<ILog>());
    }
}
