using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureBuilder;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.BeforeFixtureConstructor
{
    [TestFixtureInjectable]
    [TestMethodInjectionModule]
    [TimeLoggerModule]
    internal class Test_Methods_Inject_Method_Should
    {
        [Test]
        [Repeat(2)]
        public void Case([InjectDependency] IFixtureSetUpService service)
        {
            service.Should().NotBeNull();
        }

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(2)]
        public void Cases([InjectDependency] IFixtureSetUpService service, int value)
        {
            service.Should().NotBeNull();
        }

        [TestCaseSource(nameof(CaseValues))]
        [Repeat(2)]
        public void CaseSource([InjectDependency] IFixtureSetUpService service, int value)
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
