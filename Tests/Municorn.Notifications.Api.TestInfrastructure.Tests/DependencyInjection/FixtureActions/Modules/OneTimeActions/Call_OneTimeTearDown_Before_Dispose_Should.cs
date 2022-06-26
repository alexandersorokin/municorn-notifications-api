using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureActions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FixtureOneTimeActions;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureActions.Modules.OneTimeActions
{
    [TestFixture]
    internal sealed class Call_OneTimeTearDown_Before_Dispose_Should : IFixtureWithServiceProviderFramework, IOneTimeTearDownAction, IDisposable
    {
        private readonly Counter counter = new();

        public void ConfigureServices(IServiceCollection serviceCollection) =>
            serviceCollection.AddFixtureOneTimeActions();

        [Test]
        [Repeat(2)]
        public void Case() => this.counter.Value.Should().Be(0);

        public void OneTimeTearDown() => this.counter.Increment();

        public void Dispose() => this.counter.Value.Should().Be(1);
    }
}
