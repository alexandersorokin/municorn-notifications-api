using System;

namespace Municorn.Notifications.Api.Tests.DependencyInjection
{
    internal static class AsyncEnumerableExtensions
    {
        internal static void DisposeSynchronously(this IAsyncDisposable serviceProvider)
        {
            serviceProvider.DisposeAsync().AsTask().GetAwaiter().GetResult();
        }
    }
}
