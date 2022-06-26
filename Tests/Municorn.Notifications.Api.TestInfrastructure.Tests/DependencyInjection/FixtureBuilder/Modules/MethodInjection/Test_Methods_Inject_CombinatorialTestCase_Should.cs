using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureBuilder;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection;
using Municorn.Notifications.Api.TestInfrastructure.NUnitAttributes;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureBuilder.Modules.MethodInjection
{
    [TestFixtureInjectable]
    [TestMethodInjectionModule]
    [MockServiceScopedModule]
    internal class Test_Methods_Inject_CombinatorialTestCase_Should
    {
        [Test]
        [Repeat(2)]
        public void Case([InjectDependency] MockService service) => service.Should().NotBeNull();

        [CombinatorialTestCase(10)]
        [CombinatorialTestCase(11)]
        [Repeat(2)]
        public void Cases([InjectDependency] MockService service, int value) => service.Should().NotBeNull();

        [CombinatorialTestCaseSource(nameof(CaseValues))]
        [Repeat(2)]
        public void CaseSource(int value, [InjectDependency] MockService service) =>
            service.Should().NotBeNull();

        private static readonly TestCaseData[] CaseValues =
        {
            new(10),
            new(11),
        };
    }
}
