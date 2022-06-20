using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FluentAssertions;
using Municorn.Notifications.Api.NotificationFeature.App;
using Municorn.Notifications.Api.Tests.DependencyInjection.BeforeFixtureConstructor;
using Municorn.Notifications.Api.Tests.DependencyInjection.ScopeTestMap.AsyncLocal;
using NUnit.Framework;

namespace Municorn.Notifications.Api.Tests.IntegrationTests
{
    [TestFixtureInjectable]
    [WaiterModule]
    internal class Waiter_FixtureInjectable_ScopeMap_Should
    {
        private readonly AsyncLocalTestCaseServiceResolver<Waiter> waiter;

        public Waiter_FixtureInjectable_ScopeMap_Should(AsyncLocalTestCaseServiceResolver<Waiter> waiter) => this.waiter = waiter;

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(3)]
        public async Task Wait_Less_Than_X_Seconds(int x)
        {
            Func<Task> action = this.waiter.Value.Wait;

            await action.Should().CompleteWithinAsync(TimeSpan.FromSeconds(x)).ConfigureAwait(false);
        }

        [Test]
        [Repeat(3)]
        public async Task Wait_More_Than_500_Milliseconds()
        {
            var stopwatch = Stopwatch.StartNew();

            await this.waiter.Value.Wait().ConfigureAwait(false);

            stopwatch.Elapsed.Should().BeGreaterThan(TimeSpan.FromMilliseconds(450));
        }
    }
}
