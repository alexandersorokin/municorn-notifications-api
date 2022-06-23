using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Communication;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Communication.AsyncLocal;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.AfterFixtureConstructor.Communication.AsyncLocal
{
    [TestFixture]
    [TestCommunicationModule]
    internal class Inject_AsyncLocalExtensions_Should : ITestFixture
    {
        public void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection
            .AddScoped<ILog, SilentLog>();

        [SetUp]
        public void SetUp()
        {
            this.GetRequiredService<ILog>().Should().NotBeNull();
        }

        [TearDown]
        public void TearDown()
        {
            this.GetRequiredService<ILog>().Should().NotBeNull();
        }

        [Test]
        [Repeat(2)]
        public void Case()
        {
            this.GetRequiredService<ILog>().Should().NotBeNull();
        }

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(2)]
        public void Cases(int value)
        {
            this.GetRequiredService<ILog>().Should().NotBeNull();
        }
    }
}
