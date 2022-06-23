using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Communication;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Communication.AsyncLocal;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Fields;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.AfterFixtureConstructor.Communication.AsyncLocal
{
    [TestFixture]
    [TestCommunicationModule]
    internal class Inject_AsyncGenericProvider_Should : IWithFields
    {
        [FieldDependency]
        private readonly AsyncLocalServiceProvider<ILog> service = default!;

        public void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection
            .AddScoped<ILog, SilentLog>();

        [SetUp]
        public void SetUp()
        {
            this.service.Value.Should().NotBeNull();
        }

        [TearDown]
        public void TearDown()
        {
            this.service.Value.Should().NotBeNull();
        }

        [Test]
        [Repeat(2)]
        public void Case()
        {
            this.service.Value.Should().NotBeNull();
        }

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(2)]
        public void Cases(int value)
        {
            this.service.Value.Should().NotBeNull();
        }
    }
}
