using System;
using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureBuilder;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FixtureOneTimeActions;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureBuilder.Modules.OneTimeActions
{
    [TestFixtureInjectable]
    [FixtureOneTimeActionsModule]
    internal sealed class Call_OneTimeTearDown_Before_Dispose_Should : IOneTimeTearDownAction, IDisposable
    {
        private readonly Counter counter = new();

        [Test]
        [Repeat(2)]
        public void Case() => this.counter.Value.Should().Be(0);

        public void OneTimeTearDown() => this.counter.Increment();

        public void Dispose() => this.counter.Value.Should().Be(1);
    }
}
