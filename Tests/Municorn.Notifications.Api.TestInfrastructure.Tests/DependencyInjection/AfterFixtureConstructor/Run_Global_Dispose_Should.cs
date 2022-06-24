using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection;
using Municorn.Notifications.Api.TestInfrastructure.Logging;
using Municorn.Notifications.Api.TestInfrastructure.NUnitAttributes;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.AfterFixtureConstructor
{
    [TestFixture]
    [TestMethodInjectionModule]
    internal sealed class Run_Global_Dispose_Should : IFixtureServiceProvider, IDisposable
    {
        private readonly Counter counter = new();

        public void ConfigureServices(IServiceCollection serviceCollection) =>
            serviceCollection
                .AddContextualLog()
                .AddSingleton(this.counter)
                .AddSingleton<FixtureOneTimeTimeLogger>();

        [Test]
        [Repeat(2)]
        public void Case([InjectDependency] FixtureOneTimeTimeLogger fixtureOneTimeTimeLogger)
        {
            fixtureOneTimeTimeLogger.Run();
            true.Should().BeTrue();
        }

        [CombinatorialTestCase(10)]
        [CombinatorialTestCase(11)]
        [Repeat(2)]
        public void Cases(int value, [InjectDependency] FixtureOneTimeTimeLogger fixtureOneTimeTimeLogger)
        {
            fixtureOneTimeTimeLogger.Run();
            value.Should().BePositive();
        }

        public void Dispose()
        {
            this.counter.Value.Should().Be(1);
        }
    }
}
