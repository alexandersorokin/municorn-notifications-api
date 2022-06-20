using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.NotificationFeature.App;
using Municorn.Notifications.Api.Tests.DependencyInjection.AfterFixture;
using Municorn.Notifications.Api.Tests.DependencyInjection.Scope;
using NUnit.Framework;

namespace Municorn.Notifications.Api.Tests.IntegrationTests
{
    [TestFixture]
#pragma warning disable S2187 // TestCases should contain tests
    internal class WaiterInjected_Should : IConfigureServices
#pragma warning restore S2187 // TestCases should contain tests
    {
        public void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection.RegisterWaiter();

        [TestInjected(10, 1.1f, 100, "by")]
        [TestInjected(11, 1.2d, null)]
        [Repeat(3)]
        public void Check_Attribute<T1, T2>(
            [Inject] ThreadSafeRandomNumberGenerator injectFirst,
            [Values] bool automaticData,
            int testCaseData,
            [Inject] Waiter injectSecond,
            [Values("string", 777)] T1 automaticInfer,
            T2 testCaseInfer,
            int? testCaseDataConversion,
            string testCaseOptional = "hello")
        {
            injectSecond.Should().NotBeNull();
        }

        [TestInjected(10)]
        [TestInjected(11)]
        [Repeat(3)]
        public async Task Wait_Less_Than_N_Seconds([Inject] Waiter waiter, int n)
        {
            Func<Task> action = waiter.Wait;

            await action.Should().CompleteWithinAsync(TimeSpan.FromSeconds(n)).ConfigureAwait(false);
        }

        [TestInjected]
        [Repeat(3)]
        public async Task Wait_More_Than_500_Milliseconds()
        {
            var stopwatch = Stopwatch.StartNew();

            await this.ResolveService<Waiter>().Wait().ConfigureAwait(false);

            stopwatch.Elapsed.Should().BeGreaterThan(TimeSpan.FromMilliseconds(450));
        }
    }
}
