using System;
using System.Threading;
using JetBrains.Annotations;

namespace Municorn.Notifications.Api.NotificationFeature.App
{
    public sealed class ThreadSafeRandomNumberGenerator : IDisposable
    {
        private readonly Random globalRandom = new();
        private readonly ThreadLocal<Random> randoms;

        public ThreadSafeRandomNumberGenerator()
        {
            this.randoms = new(() =>
            {
                lock (this.globalRandom)
                {
                    return new(this.globalRandom.Next());
                }
            });
        }

        public void Dispose()
        {
            this.randoms.Dispose();
        }

        [MustUseReturnValue]
        public int Next(int maxValue) => this.randoms.Value!.Next(maxValue);
    }
}
