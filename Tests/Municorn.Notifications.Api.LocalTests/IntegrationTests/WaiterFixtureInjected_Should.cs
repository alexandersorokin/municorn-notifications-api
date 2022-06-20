using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FluentAssertions;
using Municorn.Notifications.Api.NotificationFeature.App;
using Municorn.Notifications.Api.Tests.DependencyInjection.BeforeFixture;
using Municorn.Notifications.Api.Tests.DependencyInjection.Scope;
using NUnit.Framework;

namespace Municorn.Notifications.Api.Tests.IntegrationTests
{
    [TestFixtureInjected]
    internal class WaiterFixtureInjected_Should
    {
        private readonly Waiter waiter;

        public WaiterFixtureInjected_Should(Waiter waiter)
        {
            this.waiter = waiter;
        }

        [InjectableTest(10)]
        [InjectableTest(11)]
        [Repeat(3)]
        public async Task Wait_Less_Than_N_Seconds([Inject] Waiter w, int n)
        {
            Func<Task> action = w.Wait;

            await action.Should().CompleteWithinAsync(TimeSpan.FromSeconds(n)).ConfigureAwait(false);
        }

        [Test]
        [Repeat(3)]
        public async Task Wait_More_Than_500_Milliseconds()
        {
            var stopwatch = Stopwatch.StartNew();

            await this.waiter.Wait().ConfigureAwait(false);

            stopwatch.Elapsed.Should().BeGreaterThan(TimeSpan.FromMilliseconds(450));
        }
    }
}
