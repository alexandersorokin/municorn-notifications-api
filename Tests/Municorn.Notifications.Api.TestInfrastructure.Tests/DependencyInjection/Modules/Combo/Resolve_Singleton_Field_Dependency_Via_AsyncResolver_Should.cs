using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.TestCommunication;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Modules.Combo
{
    internal class Resolve_Singleton_Field_Dependency_Via_AsyncResolver_Should : FrameworkServiceProviderFixtureBase
    {
        [FieldDependency]
        private readonly IAsyncLocalServiceProvider<MockService> service = default!;

        public Resolve_Singleton_Field_Dependency_Via_AsyncResolver_Should()
            : base(serviceCollection => serviceCollection
                .AddSingleton<ITest>(TestExecutionContext.CurrentContext.CurrentTest)
                .AddTestCommunication()
                .AddFieldInjection(typeof(Resolve_Singleton_Field_Dependency_Via_AsyncResolver_Should))
                .AddSingleton<MockService>())
        {
        }

        [OneTimeSetUp]
        public void AsyncLocal_Is_Available_In_OneTimeSetUp_From_Root_Provider() => this.EnsureServiceIsNotNull();

        [OneTimeTearDown]
        public void AsyncLocal_Is_Available_In_OneTimeTearDown_From_Root_Provider() => this.EnsureServiceIsNotNull();

        [SetUp]
        public void AsyncLocal_Is_Available_In_SetUp_From_Child_Provider() => this.EnsureServiceIsNotNull();

        [TearDown]
        public void AsyncLocal_Is_Available_In_TearDown_Provider() => this.EnsureServiceIsNotNull();

        [Test]
        [Repeat(2)]
        public void Case() => this.EnsureServiceIsNotNull();

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(2)]
        public void Cases(int value) => this.EnsureServiceIsNotNull();

        private void EnsureServiceIsNotNull() => this.service.Value.Should().NotBeNull();
    }
}
