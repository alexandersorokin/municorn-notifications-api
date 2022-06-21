using System.Threading;

namespace Municorn.Notifications.Api.Tests.DependencyInjection
{
    internal class Counter
    {
        private int value;

        internal int Value => this.value;

        internal void Increment()
        {
            Interlocked.Increment(ref this.value);
        }
    }
}
