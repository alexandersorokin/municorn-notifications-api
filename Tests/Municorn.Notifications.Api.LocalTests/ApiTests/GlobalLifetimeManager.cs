using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Municorn.Notifications.Api.Tests.ApiTests
{
    [SetUpFixture]
    internal sealed class GlobalLifetimeManager
    {
        private readonly ConcurrentBag<IAsyncDisposable> disposables = new();

        [OneTimeTearDown]
        public async Task Dispose()
        {
            foreach (var disposable in this.disposables)
            {
                await disposable.DisposeAsync().ConfigureAwait(false);
            }
        }

        internal void RegisterForDispose(IAsyncDisposable disposable) => this.disposables.Add(disposable);
    }
}
