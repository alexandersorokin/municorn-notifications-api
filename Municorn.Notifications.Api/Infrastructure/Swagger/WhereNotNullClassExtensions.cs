using System.Collections.Generic;

namespace Municorn.Notifications.Api.Infrastructure.Swagger
{
    internal static class WhereNotNullClassExtensions
    {
        internal static IEnumerable<TSource> WhereNotNull<TSource>(this IEnumerable<TSource?> source)
            where TSource : class
        {
            foreach (var element in source)
            {
                if (element is not null)
                {
                    yield return element;
                }
            }
        }
    }
}
