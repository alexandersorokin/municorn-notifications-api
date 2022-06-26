using System;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureActions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureActions.SetUpFixtures.RunOneTimeSetUp
{
    [SetUpFixture]
    internal sealed class SetUpFixture : IFixtureWithServiceProviderFramework, IDisposable
    {
        private readonly Counter counter = new();

        public void ConfigureServices(IServiceCollection serviceCollection) =>
            serviceCollection
                .AddSingleton(this.counter)
                .AddSingleton<IFixtureOneTimeSetUpService, IncrementService>();

        public void Dispose() => this.counter.Value.Should().Be(2);

        private sealed class IncrementService : IFixtureOneTimeSetUpService, IDisposable
        {
            private readonly Counter counter;

            [UsedImplicitly]
            public IncrementService(Counter counter) => this.counter = counter;

            public void Run() => this.counter.Increment();

            public void Dispose() => this.counter.Increment();
        }
    }
}
