using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureActions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection;
using Municorn.Notifications.Api.TestInfrastructure.NUnitAttributes;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureActions
{
    [TestFixture]
    internal class TestAccessor_As_Service_Should : IFixtureWithServiceProviderFramework
    {
        public void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection
            .AddTestMethodInjection()
            .AddSingleton<object>();

        [Test]
        [Repeat(2)]
        public void Case([InjectParameterDependency] TestAccessor testAccessor) =>
            testAccessor.Test.Should().Be(TestExecutionContext.CurrentContext.CurrentTest);

        [CombinatorialTestCase(10)]
        [CombinatorialTestCase(11)]
        [Repeat(2)]
        public void Cases([InjectParameterDependency] TestAccessor testAccessor, int value) =>
            testAccessor.Test.Should().Be(TestExecutionContext.CurrentContext.CurrentTest);
    }
}
