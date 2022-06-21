using System;
using System.Diagnostics;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AutoMethods;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection
{
    [PrimaryConstructor]
    internal sealed partial class FixtureTimeLogger : IFixtureOneTimeSetUp, IDisposable
    {
        private readonly Counter counter;
        private readonly ILog log;
        private Stopwatch? stopWatch;

        public void Run() => this.stopWatch = Stopwatch.StartNew();

        public void Dispose()
        {
            this.log.Info($"Fixture finished. Elapsed: {this.stopWatch?.Elapsed}");
            this.counter.Increment();
        }
    }
}