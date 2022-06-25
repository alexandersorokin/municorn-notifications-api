using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Abstractions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.TestCommunication;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Modules.FieldInjection
{
    internal class Register_Scoped_Field_Dependency_Should : FrameworkServiceProviderFixtureBase
    {
        [FieldDependency]
        [RegisterDependency]
        private readonly IAsyncLocalServiceProvider<SilentLog> service = default!;

        public Register_Scoped_Field_Dependency_Should()
            : base(serviceCollection => serviceCollection
                .AddSingleton<ITest>(TestExecutionContext.CurrentContext.CurrentTest)
                .AddTestCommunication()
                .AddFieldInjection(typeof(Register_Scoped_Field_Dependency_Should)))
        {
        }

        [Test]
        [Repeat(2)]
        public void Case()
        {
            this.service.Value.Should().NotBeNull();
        }

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(2)]
        public void Cases(int value)
        {
            this.service.Value.Should().NotBeNull();
        }
    }
}
