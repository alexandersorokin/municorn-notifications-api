using System;
using System.Reflection;

namespace Municorn.Notifications.Api.Infrastructure.Reflection
{
    internal static class TypeIsRecordExtensions
    {
        internal static bool IsRecord(this Type type)
        {
            if (type.IsAbstract || type.IsValueType || type.IsInterface)
            {
                return false;
            }

            // Based on the state of the art as described in https://github.com/dotnet/roslyn/issues/45777
            var cloneMethod = type.GetMethod("<Clone>$", BindingFlags.Public | BindingFlags.Instance);
            return cloneMethod is not null && cloneMethod.ReturnType == type;
        }
    }
}