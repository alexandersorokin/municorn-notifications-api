using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.Tests.DependencyInjection.AutoMethods;
using Municorn.Notifications.Api.Tests.Logging;
using NUnit.Framework;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.AfterFixtureConstructor.Tests.SetUpFixtures.SetUp
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
                .AddScoped<IFixtureSetUp, TestTimeLogger>();

        public void Dispose()
        {
            this.counter.Value.Should().Be(3);
        }
    }
}
