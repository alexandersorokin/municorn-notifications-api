﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FluentAssertions;
using Municorn.Notifications.Api.NotificationFeature.App;
using Municorn.Notifications.Api.Tests.DependencyInjection.BeforeFixtureConstructor;
using Municorn.Notifications.Api.Tests.DependencyInjection.ScopeMethodInject;
using NUnit.Framework;

namespace Municorn.Notifications.Api.Tests.IntegrationTests
{
    [TestFixtureInjected]
    internal class Waiter_FixtureInjectable_TestCaseCombinatorial_Should
    {
        private readonly Waiter waiter;

        public Waiter_FixtureInjectable_TestCaseCombinatorial_Should(Waiter waiter) => this.waiter = waiter;

        [CombinatorialTestCase(10)]
        [CombinatorialTestCase(11)]
        [Repeat(3)]
        public async Task Wait_Less_Than_X_Seconds([Inject] Waiter w, int x)
        {
            Func<Task> action = w.Wait;

            await action.Should().CompleteWithinAsync(TimeSpan.FromSeconds(x)).ConfigureAwait(false);
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