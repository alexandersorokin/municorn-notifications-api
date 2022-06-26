using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureActions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection;
using Municorn.Notifications.Api.TestInfrastructure.NUnitAttributes;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureActions
{
    [TestFixture]
    internal sealed class Run_Dispose_PerTest_Should : IFixtureWithServiceProviderFramework, IDisposable
    {
        private readonly Counter counter = new();

        public void ConfigureServices(IServiceCollection serviceCollection) =>
            serviceCollection
                .AddTestMethodInjection()
                .AddSingleton(this.counter)
                .AddScoped<OnDisposeIncrementService>();

        [Test]
        [Repeat(2)]
        public void Case([InjectDependency] OnDisposeIncrementService service) => service.Should().NotBeNull();

        [CombinatorialTestCase(10)]
        [CombinatorialTestCase(11)]
        [Repeat(2)]
        public void Cases(int value, [InjectDependency] OnDisposeIncrementService service)
        {
            service.Should().NotBeNull();
            value.Should().BePositive();
        }

        public void Dispose() => this.counter.Value.Should().Be(6);

        internal sealed class OnDisposeIncrementService : IDisposable
        {
            private readonly Counter counter;

            public OnDisposeIncrementService(Counter counter) => this.counter = counter;

            public void Dispose() => this.counter.Increment();
        }
    }
}
