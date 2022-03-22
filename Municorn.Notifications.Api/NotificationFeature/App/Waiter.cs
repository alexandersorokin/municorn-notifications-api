using System;
using System.Threading.Tasks;

namespace Municorn.Notifications.Api.NotificationFeature.App
{
    [PrimaryConstructor]
    public partial class Waiter
    {
        private readonly ThreadSafeRandomNumberGenerator generator;

        public async Task Wait()
        {
            var delay = this.generator.Next(2) == 0
                ? 500
                : 2000;

            await Task.Delay(TimeSpan.FromMilliseconds(delay)).ConfigureAwait(false);
        }
    }
}
