using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Combo;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.TestCommunication;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Modules.Combo
{
    internal class Register_Scoped_Field_Dependency_ImplementationType_Should : FrameworkServiceProviderFixtureBase
    {
        [InjectFieldDependency]
        [RegisterDependency(typeof(MockService))]
        private readonly IAsyncLocalServiceProvider<IMockService> service = default!;

        public Register_Scoped_Field_Dependency_ImplementationType_Should()
            : base(serviceCollection => serviceCollection
                .AddSingleton<ITest>(TestExecutionContext.CurrentContext.CurrentTest)
                .AddTestCommunication()
                .AddFieldInjection(typeof(Register_Scoped_Field_Dependency_ImplementationType_Should)))
        {
        }

        [SetUp]
        public void AsyncLocal_Is_Available_In_SetUp() => this.EnsureServiceIsNotNull();

        [TearDown]
        public void AsyncLocal_Is_Available_In_TearDown() => this.EnsureServiceIsNotNull();

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
