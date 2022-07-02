using System;
using System.Collections.Concurrent;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.TestCommunication;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Modules.FieldInjection
{
    internal sealed class Register_Scoped_Field_Dependency_AsScoped_Should : FrameworkServiceProviderFixtureBase, IDisposable
    {
        private readonly ConcurrentDictionary<MockService, bool> services = new();

        [FieldDependency]
        [RegisterDependency]
        private readonly IAsyncLocalServiceProvider<MockService> serviceProvider = default!;

        public Register_Scoped_Field_Dependency_AsScoped_Should()
            : base(serviceCollection => serviceCollection
                .AddSingleton<ITest>(TestExecutionContext.CurrentContext.CurrentTest)
                .AddTestCommunication()
                .AddFieldInjection(typeof(Register_Scoped_Field_Dependency_AsScoped_Should)))
        {
        }

        [Test]
        [Repeat(2)]
        public void Case() => this.EnsureServiceIsNotNull();

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(2)]
        public void Cases(int value) => this.EnsureServiceIsNotNull();

        public void Dispose() => this.services.Should().HaveCount(6);

        private void EnsureServiceIsNotNull()
        {
            var service = this.serviceProvider.Value;
            service.Should().NotBeNull();

            if (!this.services.TryAdd(service, true))
            {
                throw new InvalidOperationException("Same scoped service is used in different test");
            }
        }
    }
}
