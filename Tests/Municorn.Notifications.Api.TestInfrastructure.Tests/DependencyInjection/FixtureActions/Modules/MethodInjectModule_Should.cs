using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureActions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureActions.Modules
{
    [TestFixture]
    internal class MethodInjectModule_Should : IFixtureWithServiceProviderFramework
    {
        public void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection
            .AddTestMethodInjection()
            .AddScoped<MockService>();

        [Test]
        [Repeat(2)]
        public void Case([InjectParameterDependency] MockService service) => service.Should().NotBeNull();

        [Test]
        [Repeat(2)]
        public void Select_Service([InjectParameterDependency(typeof(MockService))] object service) =>
            service.Should().NotBeNull();

        [Test]
        [Repeat(2)]
        public void Select_Two_Services(
            [InjectParameterDependency(typeof(MockService)), InjectParameterDependency(typeof(MethodInjectModule_Should))] object service) =>
            service.Should().NotBeNull();

        [Test]
        [Repeat(2)]
        public void Case_With_Provider([InjectParameterDependency] MockService service, [Values] bool value) => service.Should().NotBeNull();

        [TestCaseSource(nameof(CaseValues))]
        [Repeat(2)]
        public void Cases(int value, MockService service) => service.Should().NotBeNull();

        private static readonly TestCaseData[] CaseValues =
        {
            CreateCase(10),
            CreateCase(11),
        };

        private static TestCaseData CreateCase(int value) => new(value, new InjectedService<MockService>());
    }
}
