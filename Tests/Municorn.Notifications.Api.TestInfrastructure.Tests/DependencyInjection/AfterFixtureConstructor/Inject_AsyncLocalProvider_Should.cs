using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.ScopeAsyncLocal;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.AfterFixtureConstructor
{
    [TestFixture]
    internal class Inject_AsyncLocalProvider_Should : IConfigureServices
    {
        [TestDependency]
        private readonly AsyncLocalTestCaseServiceResolver resolver = default!;

        public void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection
            .AddScoped<ILog, SilentLog>();

        [SetUp]
        public void SetUp()
        {
            this.resolver.ResolveService<ILog>().Should().NotBeNull();
        }

        [TearDown]
        public void TearDown()
        {
            this.resolver.ResolveService<ILog>().Should().NotBeNull();
        }

        [Test]
        [Repeat(2)]
        public void Case()
        {
            this.resolver.ResolveService<ILog>().Should().NotBeNull();
        }

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(2)]
        public void Cases(int value)
        {
            this.resolver.ResolveService<ILog>().Should().NotBeNull();
        }
    }
}
