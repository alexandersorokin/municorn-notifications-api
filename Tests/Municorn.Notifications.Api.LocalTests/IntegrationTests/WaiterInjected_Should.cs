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

        [TestInjected]
        [Repeat(3)]
        public async Task Wait_Less_Than_10_Seconds(Waiter waiter)
        {
            Func<Task> action = waiter.Wait;

            await action.Should().CompleteWithinAsync(TimeSpan.FromSeconds(10)).ConfigureAwait(false);
        }

        [TestInjected]
        [Repeat(3)]
        public async Task Wait_More_Than_500_Milliseconds(Waiter waiter)
        {
            var stopwatch = Stopwatch.StartNew();

            await waiter.Wait().ConfigureAwait(false);

            stopwatch.Elapsed.Should().BeGreaterThan(TimeSpan.FromMilliseconds(450));
        }
    }
}
