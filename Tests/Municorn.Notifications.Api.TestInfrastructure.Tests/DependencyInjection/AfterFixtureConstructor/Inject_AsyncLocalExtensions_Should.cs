using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.ScopeAsyncLocal;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.AfterFixtureConstructor
{
    [TestFixture]
    internal class Inject_AsyncLocalExtensions_Should : IConfigureServices
    {
        public void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection
            .AddScoped<ILog, SilentLog>();

        [SetUp]
        public void SetUp()
        {
            this.ResolveService<ILog>().Should().NotBeNull();
        }

        [TearDown]
        public void TearDown()
        {
            this.ResolveService<ILog>().Should().NotBeNull();
        }

        [Test]
        [Repeat(2)]
        public void Case()
        {
            this.ResolveService<ILog>().Should().NotBeNull();
        }

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(2)]
        public void Cases(int value)
        {
            this.ResolveService<ILog>().Should().NotBeNull();
        }
    }
}
