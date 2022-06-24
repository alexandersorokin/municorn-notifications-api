using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Fields;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.AfterFixtureConstructor.Modules.Fields
{
    [TestFixture]
    [FieldDependenciesModule]
    internal class Inject_Field_Should : IFixtureServiceProvider
    {
        [FieldDependency]
        private readonly ILog service = default!;

        public void ConfigureServices(IServiceCollection serviceCollection) =>
            serviceCollection.AddSingleton<ILog, SilentLog>();

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
