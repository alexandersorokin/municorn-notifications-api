using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.AfterFixtureConstructor.Tests
{
    [TestFixture]
    internal class Inject_Field_Should : IConfigureServices
    {
        [TestDependency]
        private readonly ILog service = default!;

        public void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection
            .AddSingleton<ILog, SilentLog>();

        [Test]
        [Repeat(2)]
        public void Case()
        {
            this.service.Should().NotBeNull();
        }

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(2)]
        public void Cases(int value)
        {
            this.service.Should().NotBeNull();
        }
    }
}
