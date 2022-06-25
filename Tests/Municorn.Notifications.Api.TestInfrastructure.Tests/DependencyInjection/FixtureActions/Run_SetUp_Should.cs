using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureActions
{
    [TestFixture]
    internal sealed class Run_SetUp_Should : IWithFields, IDisposable
    {
        [FieldDependency]
        private readonly Counter counter = default!;

        public void SetUpServices(IServiceCollection serviceCollection) => serviceCollection
            .AddContextualLog()
            .AddTestTimeLogger();

        [Test]
        [Repeat(2)]
        public void Case() => true.Should().BeTrue();

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(2)]
        public void Cases(int value) => value.Should().BePositive();

        public void Dispose() => this.counter.Value.Should().Be(6);
    }
}
