using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.Tests.DependencyInjection.AutoMethods;
using Municorn.Notifications.Api.Tests.Logging;
using NUnit.Framework;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.AfterFixtureConstructor.Tests
{
    [TestFixture]
    internal sealed class Run_SetUp_Should : IConfigureServices, IDisposable
    {
        [TestDependency]
        private readonly Counter counter = default!;

        public void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection
            .AddContextualLog()
            .AddSingleton<Counter>()
            .AddScoped<IFixtureSetUp, TestTimeLogger>();

        [Test]
        [Repeat(2)]
        public void Case()
        {
            true.Should().BeTrue();
        }

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(2)]
        public void Cases(int value)
        {
            value.Should().BePositive();
        }

        public void Dispose()
        {
            this.counter.Value.Should().Be(6);
        }
    }
}
