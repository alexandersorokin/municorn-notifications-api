using System;
using System.Diagnostics;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Scopes;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection
{
    [PrimaryConstructor]
    internal sealed partial class TestTimeLogger : IFixtureSetUp, IDisposable
    {
        private readonly Counter counter;
        private readonly TestAccessor testAccessor;
        private readonly ILog log;
        private Stopwatch? stopWatch;

        public void Run() => this.stopWatch = Stopwatch.StartNew();

        public void Dispose()
        {
            var stopwatch = this.stopWatch ?? throw new InvalidOperationException("Run wasn't called");
            this.log.Info($"Test {this.testAccessor.Test.FullName} finished. Elapsed: {stopwatch.Elapsed}");
            this.counter.Increment();
        }
    }
}