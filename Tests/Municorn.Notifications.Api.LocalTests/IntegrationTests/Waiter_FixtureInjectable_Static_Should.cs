using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FluentAssertions;
using Municorn.Notifications.Api.NotificationFeature.App;
using Municorn.Notifications.Api.Tests.DependencyInjection.BeforeFixtureConstructor;
using NUnit.Framework;

namespace Municorn.Notifications.Api.Tests.IntegrationTests
{
    [TestFixtureInjectable]
    [WaiterModule]
    internal class Waiter_FixtureInjectable_Static_Should
    {
        private readonly Waiter waiter;

        public Waiter_FixtureInjectable_Static_Should(Waiter waiter) => this.waiter = waiter;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            false.Should().BeTrue();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            false.Should().BeTrue();
        }

        [SetUp]
        public void SetUp()
        {
            true.Should().BeTrue();
        }

        [TearDown]
        public void TearDown()
        {
            true.Should().BeTrue();
        }

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(3)]
        public async Task Wait_Less_Than_N_Seconds(int n)
        {
            Func<Task> action = this.waiter.Wait;

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
