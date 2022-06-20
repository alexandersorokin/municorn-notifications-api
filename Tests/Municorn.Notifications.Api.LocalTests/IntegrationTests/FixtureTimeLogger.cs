using System;
using System.Diagnostics;
using Municorn.Notifications.Api.Tests.DependencyInjection;
using Municorn.Notifications.Api.Tests.DependencyInjection.AfterFixtureConstructor;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.Tests.IntegrationTests
{
    [PrimaryConstructor]
    internal sealed partial class FixtureTimeLogger : IFixtureOneTimeSetUp, IDisposable
    {
        private readonly IFixtureProvider fixtureProvider;
        private readonly ILog log;
        private Stopwatch? stopWatch;

        public void Run() => this.stopWatch = Stopwatch.StartNew();

        public void Dispose() =>
            this.log.Info($"Fixture {this.fixtureProvider.Fixture} finished. Elapsed: {this.stopWatch?.Elapsed}");
    }
}