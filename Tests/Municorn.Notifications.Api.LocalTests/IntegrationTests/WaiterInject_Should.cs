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
    internal class WaiterInject_Should : IConfigureServices
    {
        public void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection.RegisterWaiter();

        [Test]
        [Repeat(3)]
        public async Task Wait_More_Than_500_Milliseconds([Inject] Waiter waiter)
        {
            var stopwatch = Stopwatch.StartNew();

            await waiter.Wait().ConfigureAwait(false);

            stopwatch.Elapsed.Should().BeGreaterThan(TimeSpan.FromMilliseconds(450));
        }

        [TestCaseSource(nameof(Cases))]
        [Repeat(3)]
        public async Task Wait_Less_Than_N_Seconds(int n, [Inject] Waiter waiter)
        {
            Func<Task> action = waiter.Wait;

            await action.Should().CompleteWithinAsync(TimeSpan.FromSeconds(n)).ConfigureAwait(false);
        }

        private static readonly TestCaseData[] Cases =
        {
            CreateCase(10),
            CreateCase(11),
        };

        private static TestCaseData CreateCase(int n) => new(n, new InjectedService<Waiter>());
    }
}
