using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.Tests.DependencyInjection.AfterFixtureConstructor;
using Municorn.Notifications.Api.Tests.DependencyInjection.AutoMethods;
using Municorn.Notifications.Api.Tests.Logging;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.BeforeFixtureConstructor.Tests.SetUpFixtures
{
    [SetUpFixture]
    internal sealed class SetUpFixture : IConfigureServices, IDisposable
    {
        [TestDependency]
        private readonly Counter counter = default!;

        public void ConfigureServices(IServiceCollection serviceCollection) =>
            serviceCollection
                .AddBoundLog()
                .AddSingleton<Counter>()
                .AddSingleton<IFixtureOneTimeSetUp, FixtureTimeLogger>()
                .AddScoped<IFixtureSetUp, TestTimeLogger>();

        [field: TestDependency]
        internal ILog BoundLog { get; } = default!;

        public void Dispose()
        {
            this.counter.Value.Should().Be(5);
        }
    }
}
