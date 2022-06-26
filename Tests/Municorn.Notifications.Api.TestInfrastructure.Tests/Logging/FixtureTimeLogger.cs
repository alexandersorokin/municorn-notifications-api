using System;
using System.Diagnostics;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.Logging
{
    [PrimaryConstructor]
    internal sealed partial class FixtureOneTimeTimeLogger : IFixtureOneTimeSetUpService, IDisposable
    {
        private readonly ILog log;
        private Stopwatch? stopWatch;

        public void Run() => this.stopWatch = Stopwatch.StartNew();

        public void Dispose()
        {
            var stopwatch = this.stopWatch ?? throw new InvalidOperationException("Run wasn't called");
            this.log.Info($"Fixture finished. Elapsed: {stopwatch.Elapsed}");
        }
    }
}