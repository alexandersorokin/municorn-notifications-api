using System;
using FluentAssertions;
using Municorn.Notifications.Api.Tests.ApiTests;
using NUnit.Framework;

namespace Municorn.Notifications.Api.Tests.UnitTests
{
    [TestFixture]
    internal class Global_Cache_Should
    {
        [Test]
        public void Should_return_same_value()
        {
            GlobalCache cache = new();

            var instance1 = cache.GetOrCreate(() => new object());
            var instance2 = cache.GetOrCreate(() => new object());

            instance1.Should().Be(instance2);
        }

        [Test]
        public void Should_call_factory_once()
        {
            GlobalCache cache = new();
            cache.GetOrCreate(() => new object());

            Action action = () => cache.GetOrCreate<object>(() => throw new InvalidOperationException("Should not be called"));

            action.Should().NotThrow();
        }
    }
}
