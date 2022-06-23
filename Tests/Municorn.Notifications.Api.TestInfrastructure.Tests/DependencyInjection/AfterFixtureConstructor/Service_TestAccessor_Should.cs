using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection;
using Municorn.Notifications.Api.TestInfrastructure.NUnitAttributes;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.AfterFixtureConstructor
{
    [TestFixture]
    [TestMethodInjectionModule]
    internal class Service_TestAccessor_Should : ITestFixture
    {
        public void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection.AddSingleton<object>();

        [Test]
        [Repeat(2)]
        public void Case([InjectDependency] TestAccessor testAccessor)
        {
            testAccessor.Test.Should().Be(TestExecutionContext.CurrentContext.CurrentTest);
        }

        [CombinatorialTestCase(10)]
        [CombinatorialTestCase(11)]
        [Repeat(2)]
        public void Cases([InjectDependency] TestAccessor testAccessor, int value)
        {
            testAccessor.Test.Should().Be(TestExecutionContext.CurrentContext.CurrentTest);
        }
    }
}
