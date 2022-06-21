using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.AfterFixtureConstructor.Tests.SetUpFixtures.Service
{
    [SetUpFixture]
    internal class SetUpFixture : IConfigureServices
    {
        public void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection
            .AddSingleton<ILog, SilentLog>();

        [field: TestDependency]
        internal ILog Service { get; } = default!;
    }
}
