using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Communicat1ion;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Scopes.Inject;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.AfterFixtureConstructor
{
    [TestFixture]
    [TestMethodInjectionModule]
    internal class Inject_Method_Should : ITestFixture
    {
        public void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection
            .AddScoped<ILog, SilentLog>();

        [Test]
        [Repeat(2)]
        public void Case([InjectDependency] ILog service)
        {
            service.Should().NotBeNull();
        }

        [Test]
        [Repeat(2)]
        public void Select_Service([InjectDependency(typeof(ILog))] object service)
        {
            service.Should().NotBeNull();
        }

        [Test]
        [Repeat(2)]
        public void Select_Two_Services([InjectDependency(typeof(ILog)), InjectDependency(typeof(Inject_Method_Should))] object service)
        {
            service.Should().NotBeNull();
        }

        [Test]
        [Repeat(2)]
        public void Case_With_Provider([InjectDependency] ILog service, [Values] bool value)
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
