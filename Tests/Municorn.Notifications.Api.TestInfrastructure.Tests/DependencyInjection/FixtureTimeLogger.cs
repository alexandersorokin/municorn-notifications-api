using System;
using System.Diagnostics;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.AutoMethods
{
    [PrimaryConstructor]
    internal sealed partial class FixtureTimeLogger : IFixtureOneTimeSetUp, IDisposable
    {
        private readonly Counter counter;
        private readonly IFixtureProvider fixtureProvider;
        private readonly ILog log;
        private Stopwatch? stopWatch;

        public void Run() => this.stopWatch = Stopwatch.StartNew();

        public void Dispose()
        {
            this.log.Info($"Fixture {this.fixtureProvider.Fixture} finished. Elapsed: {this.stopWatch?.Elapsed}");
            this.counter.Increment();
        }
    }
}