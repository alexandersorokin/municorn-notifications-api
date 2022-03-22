using System.Collections.Generic;
using System.Reflection;

namespace Municorn.Notifications.Api.Infrastructure.Reflection
{
    internal interface IPropertiesProvider
    {
        IReadOnlyList<PropertyInfo> GetProperties(IReflect type);
    }
}
