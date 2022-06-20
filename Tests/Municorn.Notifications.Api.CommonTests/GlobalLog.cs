using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.Tests.DependencyInjection.AfterFixture;
using Municorn.Notifications.Api.Tests.Logging;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.Tests
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
        internal ILog BoundLog { get; } = default!;

        private class AdHocTextWriterProvider : ITextWriterProvider
        {
            private readonly TextWriter textWriter;

            public AdHocTextWriterProvider(TextWriter textWriter) => this.textWriter = textWriter;

            public TextWriter Get() => this.textWriter;
        }
    }
}
