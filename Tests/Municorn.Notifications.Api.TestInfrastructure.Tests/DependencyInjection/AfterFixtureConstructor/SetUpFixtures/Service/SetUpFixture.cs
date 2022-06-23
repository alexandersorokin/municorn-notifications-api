using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor.Fields;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.AfterFixtureConstructor.SetUpFixtures.Service
{
    [SetUpFixture]
    internal class SetUpFixture : ITestFixture
    {
        public void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection
            .AddSingleton<ILog, SilentLog>();

        [field: TestDependency]
        internal ILog Service { get; } = default!;
    }
}
