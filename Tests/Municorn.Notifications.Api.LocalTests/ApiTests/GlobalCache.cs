using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Municorn.Notifications.Api.Tests.ApiTests
{
    [SetUpFixture]
    internal class GlobalCache
    {
        private readonly ConcurrentDictionary<Type, Lazy<Task<object>>> cache = new();

        internal async Task<T> GetOrCreate<T>(Func<Task<T>> factory)
            where T : notnull
        {
            Lazy<Task<object>> lazy = new(async () => await factory().ConfigureAwait(false));
            var result = await this.cache
                .GetOrAdd(typeof(T), lazy)
                .Value
                .ConfigureAwait(false);
            return (T)result;
        }
    }
}
