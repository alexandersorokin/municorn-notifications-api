using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureActions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Combo;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FixtureOneTimeActions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureActions.Integration
{
    [TestFixture]
    internal sealed class TestFixture_Should : IWithDefaultModules, IOneTimeSetUpAction, IDisposable
    {
        private readonly Counter setUpActionCounter = new();

        [FieldDependency]
        [RegisterDependency(typeof(MockService))]
        private readonly IMockService fieldService = default!;

        public void SetUpServices(IServiceCollection serviceCollection) => serviceCollection
            .AddTestMethodInjection()
            .AddScoped<MockService>();

        [Test]
        [Repeat(2)]
        public void Inject_Field() => this.fieldService.Should().NotBeNull();

        [Test]
        [Repeat(2)]
        public void Inject_Service([InjectDependency] MockService service) => service.Should().NotBeNull();

        [Test]
        [Repeat(2)]
        public void Resolve_Service_Via_AsyncLocal() =>
            this.GetRequiredService<MockService>().Should().NotBeNull();

        [Test]
        [Repeat(2)]
        public void Inject_SetUpFixture([InjectDependency] SetUpFixture setUpFixture) =>
            setUpFixture.Service.Should().NotBeNull();

        [Test]
        [Repeat(2)]
        public void Inject_SetUpFixture_Service_Via_AsyncLocal([InjectDependency] SetUpFixture setUpFixture) =>
            setUpFixture.GetRequiredService<MockService>().Should().Be(setUpFixture.Service);

        public void OneTimeSetUp()
        {
            this.fieldService.Should().NotBeNull();
            this.setUpActionCounter.Increment();
        }

        public void Dispose() => this.setUpActionCounter.Value.Should().Be(1);
    }
}
