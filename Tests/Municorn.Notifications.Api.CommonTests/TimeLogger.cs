using System;
using System.Diagnostics;
using Municorn.Notifications.Api.Tests.DependencyInjection;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.Tests
{
    [PrimaryConstructor]
    internal sealed partial class TimeLogger : IFixtureSetUp, IDisposable
    {
        private readonly TestAccessor testAccessor;
        private readonly ILog log;
        private Stopwatch? stopWatch;

        public void SetUp() => this.stopWatch = Stopwatch.StartNew();

        public void Dispose() =>
            this.log.Info($"Test {this.testAccessor.Test.FullName} finished. Elapsed: {this.stopWatch?.Elapsed}");
    }
}