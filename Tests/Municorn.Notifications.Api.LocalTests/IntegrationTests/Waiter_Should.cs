using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.NotificationFeature.App;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection;
using NUnit.Framework;

namespace Municorn.Notifications.Api.Tests.IntegrationTests
{
    [TestFixture]
    internal class Waiter_Should : IFixtureWithServiceProviderFramework
    {
        [FieldDependency]
        private readonly Waiter waiter = default!;

        public void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection
            .AddFieldInjection(this)
            .AddWaiter();

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(3)]
        public async Task Wait_Less_Than_N_Seconds(int seconds)
        {
            Func<Task> action = this.waiter.Wait;

            await action.Should().CompleteWithinAsync(TimeSpan.FromSeconds(seconds)).ConfigureAwait(false);
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
