using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.TestCommunication;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Modules.TestCommunication
{
    internal class Provide_Scoped_Service_Should : FrameworkServiceProviderFixtureBase
    {
        public Provide_Scoped_Service_Should()
            : base(serviceCollection => serviceCollection
                .AddTestCommunication()
                .AddSingleton<ITest>(TestExecutionContext.CurrentContext.CurrentTest)
                .AddScoped<MockService>())
        {
        }

        [SetUp]
        public void Provide_Service_Provider_Via_Test_SetUp() => this.EnsureResolved();

        [TearDown]
        public void Provide_Service_Provider_Via_Test_TearDown() => this.EnsureResolved();

        [Test]
        public void Provide_Service_Provider_Via_Test() => this.EnsureResolved();

        [Test]
        [Repeat(3)]
        public void Provide_Service_Provider_Via_Test_Repeated() => this.EnsureResolved();

        private void EnsureResolved() =>
            TestExecutionContext.CurrentContext.CurrentTest
                .GetServiceProvider(this)
                .GetRequiredService<MockService>()
                .Should()
                .NotBeNull();
    }
}
