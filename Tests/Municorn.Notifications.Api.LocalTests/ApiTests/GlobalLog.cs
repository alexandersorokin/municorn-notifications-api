using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.Tests.DependencyInjection;
using Municorn.Notifications.Api.Tests.Log;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.Tests.ApiTests
{
    [SetUpFixture]
    internal class GlobalLog : IConfigureServices
    {
        public void ConfigureServices(IServiceCollection serviceCollection) =>
            serviceCollection
                .AddSingleton(TestContext.Out)
                .AddSingleton<ITextWriterProvider, AdHocTextWriterProvider>()
                .AddSingleton<ILog, TextWriterLog>();

        [field: TestDependency]
        public ILog BoundLog { get; } = default!;
    }
}
