using System;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureActions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureBuilder.SetUpFixtures.AfterFixtureConstructorCountChilds
{
    [SetUpFixture]
    internal sealed class SetUpFixture : IFixtureWithServiceProviderFramework, IDisposable
    {
        private readonly Counter counter = new();

        public void ConfigureServices(IServiceCollection serviceCollection) =>
            serviceCollection
                .AddTestMethodInjection()
                .AddFieldInjection(this)
                .AddSingleton<IMockService, MockService>()
                .AddSingleton(this.counter)
                .AddSingleton<IFixtureOneTimeSetUpService, IncrementService>()
                .AddScoped<IFixtureSetUpService, ScopedIncrementService>();

        [field: FieldDependency]
        public IMockService Service { get; } = default!;

        public void Dispose() => this.counter.Value.Should().Be(14);

        private sealed class IncrementService : IFixtureOneTimeSetUpService, IDisposable
        {
            private readonly Counter counter;

            [UsedImplicitly]
            public IncrementService(Counter counter) => this.counter = counter;

            public void Run() => this.counter.Increment();

            public void Dispose() => this.counter.Increment();
        }

        private sealed class ScopedIncrementService : IFixtureSetUpService, IDisposable
        {
            private readonly Counter counter;

            [UsedImplicitly]
            public ScopedIncrementService(Counter counter) => this.counter = counter;

            public void Run() => this.counter.Increment();

            public void Dispose() => this.counter.Increment();
        }
    }
}
