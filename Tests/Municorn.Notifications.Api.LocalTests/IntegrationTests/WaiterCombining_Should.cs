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
    internal class WaiterCombining_Should : IConfigureServices
#pragma warning restore S2187 // TestCases should contain tests
    {
        public void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection.RegisterWaiter();

        [TestCaseCombining(10, 1.1f, 100)]
        [TestCaseCombining(11, 1.2d, null)]
        [Repeat(3)]
        public void Check_Attribute<T1, T2>(
            [Inject(typeof(ThreadSafeRandomNumberGenerator))] object injectFirst,
            [Values] bool automaticData,
            int testCaseData,
            [Inject] Waiter injectSecond,
            [Values("string", 777)] T1 automaticInfer,
            T2 testCaseInfer,
            int? testCaseDataConversion,
            [Values(true, null)] bool? valuesConversion,
            [Inject] GlobalLog log)
        {
            injectSecond.Should().NotBeNull();
        }

        [TestCaseCombining(10, "by")]
        [TestCaseCombining(11)]
        [Repeat(3)]
        public async Task Wait_Less_Than_N_Seconds([Inject] Waiter waiter, int n, string x = "c")
        {
            Func<Task> action = waiter.Wait;

            await action.Should().CompleteWithinAsync(TimeSpan.FromSeconds(n)).ConfigureAwait(false);
        }

        [TestCaseCombining]
        [Repeat(3)]
        public async Task Wait_More_Than_500_Milliseconds()
        {
            var stopwatch = Stopwatch.StartNew();

            await this.ResolveService<Waiter>().Wait().ConfigureAwait(false);

            stopwatch.Elapsed.Should().BeGreaterThan(TimeSpan.FromMilliseconds(450));
        }
    }
}
