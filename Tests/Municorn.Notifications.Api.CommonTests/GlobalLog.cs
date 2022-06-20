using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.Tests.DependencyInjection;
using Municorn.Notifications.Api.Tests.DependencyInjection.AfterFixtureConstructor;
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
                .AddSingleton<ILog, TextWriterLog>()
                .AddScoped<IFixtureSetUp, TimeLogger>();

        [field: TestDependency]
        internal ILog BoundLog { get; } = default!;

        private class AdHocTextWriterProvider : ITextWriterProvider
        {
            private readonly TextWriter textWriter;

            public AdHocTextWriterProvider(TextWriter textWriter) => this.textWriter = textWriter;

            public TextWriter Get() => this.textWriter;
        }

        private sealed class TimeLogger : IFixtureSetUp, IDisposable
        {
            private readonly ILog log;
            private Stopwatch? stopWatch;

            public TimeLogger(ILog log) => this.log = log;

            public void SetUp() => this.stopWatch = Stopwatch.StartNew();

            public void Dispose()
            {
                this.log.Info($"Test finished. Elapsed: {this.stopWatch?.Elapsed}");
            }
        }
    }
}
