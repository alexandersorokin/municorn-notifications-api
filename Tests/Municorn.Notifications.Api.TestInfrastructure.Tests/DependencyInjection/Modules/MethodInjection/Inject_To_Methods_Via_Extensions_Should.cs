using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection;
using Municorn.Notifications.Api.TestInfrastructure.NUnitAttributes;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Modules.MethodInjection
{
    internal class Inject_To_Methods_Via_Extensions_Should : FrameworkServiceProviderFixtureBase
    {
        public Inject_To_Methods_Via_Extensions_Should()
            : base(serviceCollection => serviceCollection
                .AddTestMethodInjection()
                .AddSingleton<MockService>())
        {
        }

        [Test]
        public void Plain_Test() => true.Should().BeTrue();

        [TestCase(1)]
        [TestCase(2)]
        public void Plain_TestCase(int value) => value.Should().BePositive();

        [Test]
        public void Simple_Inject([InjectParameterDependency] MockService service) => service.Should().NotBeNull();

        [Test]
        [Repeat(3)]
        public void Repeat_Inject([InjectParameterDependency] MockService service) => service.Should().NotBeNull();

        [Test]
        public void Select_Service([InjectParameterDependency(typeof(MockService))] IMockService service) => service.Should().NotBeNull();

        [Test]
        public void Select_Two_Services([InjectParameterDependency(typeof(MockService)), InjectParameterDependency(typeof(IFixtureSetUpService))] object service) =>
            service.Should().NotBeNull();

        [Test]
        public void Case_With_Provider([InjectParameterDependency] MockService service, [Values] bool value) =>
            service.Should().NotBeNull();

        private static readonly TestCaseData[] CaseValues =
        {
            CreateMarkerCase(10),
            CreateMarkerCase(11),
        };

        [TestCaseSource(nameof(CaseValues))]
        public void Cases_With_Marker_Created_Manually(int value, MockService service) => service.Should().NotBeNull();

        [CombinatorialTestCase]
        public void CombinatorialTestCase_Inject([InjectParameterDependency] MockService service) => service.Should().NotBeNull();

        [CombinatorialTestCase(1)]
        [CombinatorialTestCase(2)]
        public void TestCases_Inject([InjectParameterDependency] MockService service, int value)
        {
            service.Should().NotBeNull();
            value.Should().BePositive();
        }

        private static readonly TestCaseData[] CombinatorialCaseSourceValues =
        {
            new(10),
            new(11),
        };

        [CombinatorialTestCaseSource(nameof(CombinatorialCaseSourceValues))]
        public void Cases(int value, [InjectParameterDependency] MockService service) => service.Should().NotBeNull();

        private static TestCaseData CreateMarkerCase(int value) => new(value, new InjectedService<MockService>());
    }
}
