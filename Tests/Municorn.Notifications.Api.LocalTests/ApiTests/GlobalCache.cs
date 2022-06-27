using System;
using System.Collections.Concurrent;
using NUnit.Framework;

namespace Municorn.Notifications.Api.Tests.ApiTests
{
    [SetUpFixture]
    internal class GlobalCache
    {
        private readonly ConcurrentDictionary<Type, Lazy<object>> cache = new();

        internal T GetOrCreate<T>(Func<T> factory)
            where T : notnull
        {
            Lazy<object> lazy = new(() => factory());
            return (T)this.cache.GetOrAdd(typeof(T), lazy).Value;
        }
    }
}