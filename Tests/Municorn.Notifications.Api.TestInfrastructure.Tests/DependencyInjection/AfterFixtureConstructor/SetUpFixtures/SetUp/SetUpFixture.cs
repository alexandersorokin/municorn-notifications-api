using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.AfterFixtureConstructor.SetUpFixtures.SetUp
{
    [SetUpFixture]
    internal sealed class SetUpFixture : IWithFields, IDisposable
    {
        [FieldDependency]
        private readonly Counter counter = default!;

        public void ConfigureServices(IServiceCollection serviceCollection) =>
            serviceCollection
                .AddBoundLog()
                .AddTestTimeLogger();

        public void Dispose()
        {
            this.counter.Value.Should().Be(3);
        }
    }
}
