using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace Municorn.Notifications.Api.Infrastructure.Reflection
{
    internal class CachedPropertiesProvider : IPropertiesProvider
    {
        private readonly ConcurrentDictionary<IReflect, IReadOnlyList<PropertyInfo>> cache = new();

        public IReadOnlyList<PropertyInfo> GetProperties(IReflect type)
        {
            return this.cache.GetOrAdd(
                type,
                t => t.GetProperties(BindingFlags.Public | BindingFlags.Instance));
        }
    }
}
