using System;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.AfterFixture
{
    internal static class AsyncEnumerableExtensions
    {
        internal static void DisposeSynchronously(this IAsyncDisposable serviceProvider) => serviceProvider.DisposeAsync().AsTask().GetAwaiter().GetResult();
    }
}
