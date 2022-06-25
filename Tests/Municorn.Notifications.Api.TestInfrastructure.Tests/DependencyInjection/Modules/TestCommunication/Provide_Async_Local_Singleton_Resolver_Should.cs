using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.TestCommunication;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Modules.TestCommunication
{
    internal class Provide_Async_Local_Singleton_Resolver_Should : FrameworkServiceProviderFixtureBase
    {
        private IAsyncLocalServiceProvider? serviceProvider;

        public Provide_Async_Local_Singleton_Resolver_Should()
            : base(serviceCollection => serviceCollection
                .AddTestCommunication()
                .AddSingleton<ITest>(TestExecutionContext.CurrentContext.CurrentTest)
                .AddSingleton<SilentLog>())
        {
        }

        [OneTimeSetUp]
        public void Provide_Service_Provider_Via_TestFixture_SetUp()
        {
            this.serviceProvider = TestExecutionContext.CurrentContext.CurrentTest
                .GetServiceProvider(this)
                .GetRequiredService<IAsyncLocalServiceProvider>();

            this.EnsureResolved();
        }

        [SetUp]
        public void Provide_Service_Provider_Via_Test_SetUp() => this.EnsureResolved();

        [TearDown]
        public void Provide_Service_Provider_Via_Test_TearDown() => this.EnsureResolved();

        [OneTimeTearDown]
        public void Provide_Service_Provider_Via_TestFixture_TearDown() => this.EnsureResolved();

        [Test]
        public void Provide_Service_Provider_Via_Test() => this.EnsureResolved();

        [Test]
        [Repeat(3)]
        public void Provide_Service_Provider_Via_Test_Repeated() => this.EnsureResolved();

        private void EnsureResolved()
        {
            var provider = this.serviceProvider ?? throw new InvalidOperationException("Provider should be initialized");
            provider.GetRequiredService<SilentLog>().Should().NotBeNull();
        }
    }
}
