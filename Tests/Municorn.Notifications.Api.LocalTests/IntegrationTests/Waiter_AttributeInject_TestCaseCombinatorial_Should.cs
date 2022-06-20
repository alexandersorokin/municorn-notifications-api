using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.NotificationFeature.App;
using Municorn.Notifications.Api.Tests.DependencyInjection.AfterFixture;
using Municorn.Notifications.Api.Tests.DependencyInjection.ScopeMethodInject;
using NUnit.Framework;

namespace Municorn.Notifications.Api.Tests.IntegrationTests
{
    [TestFixture]
    internal class Waiter_AttributeInject_TestCaseCombinatorial_Should : IConfigureServices
    {
        [TestDependency]
        private readonly Waiter waiter = default!;

        public void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection.RegisterWaiter();

        [CombinatorialTestCase(10)]
        [CombinatorialTestCase(11)]
        [Repeat(3)]
        public async Task Wait_Less_Than_N_Seconds(int n, [Inject] Waiter waiter)
        {
            Func<Task> action = waiter.Wait;

            await action.Should().CompleteWithinAsync(TimeSpan.FromSeconds(n)).ConfigureAwait(false);
        }

        [CombinatorialTestCase]
        [Repeat(3)]
        public async Task Wait_More_Than_490_Milliseconds([Inject] Waiter w)
        {
            var stopwatch = Stopwatch.StartNew();

            await w.Wait().ConfigureAwait(false);

            stopwatch.Elapsed.Should().BeGreaterThan(TimeSpan.FromMilliseconds(440));
        }

        [CombinatorialTestCase]
        [Repeat(3)]
        public async Task Wait_More_Than_500_Milliseconds()
        {
            var stopwatch = Stopwatch.StartNew();

            await this.waiter.Wait().ConfigureAwait(false);

            stopwatch.Elapsed.Should().BeGreaterThan(TimeSpan.FromMilliseconds(450));
        }
    }
}
