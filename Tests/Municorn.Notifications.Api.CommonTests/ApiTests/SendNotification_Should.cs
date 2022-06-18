using System.Threading.Tasks;
using FluentAssertions;
using Kontur.Results;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.Tests.DependencyInjection;
using NUnit.Framework;

namespace Municorn.Notifications.Api.Tests.ApiTests
{
    [TestFixture]
    internal class SendNotification_Should : IConfigureServices
    {
        [TestDependency]
        private readonly ClientFactory clientFactory = default!;

        public void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection.RegisterClientFactory();

        public static readonly SendNotificationRequest[] CorrectNotifications =
        {
            new AndroidSendNotificationRequest("token", "MessageA", "title1"),
            new AndroidSendNotificationRequest("token", "MessageB", "title2")
            {
                Condition = "cond",
            },
            new IosSendNotificationRequest("tokenA", "alert1"),
            new IosSendNotificationRequest("tokenB", "alert2")
            {
                IsBackground = false,
            },
            new IosSendNotificationRequest("tokenC", "alert3")
            {
                Priority = 500,
            },
            new IosSendNotificationRequest("tokenD", "alert4")
            {
                Priority = 5000,
                IsBackground = false,
            },
        };

        [Test]
        public async Task Succeed([ValueSource(nameof(CorrectNotifications))] SendNotificationRequest notificationRequest)
        {
            var client = await this.clientFactory.Create().ConfigureAwait(false);

            var sendResult = await client.SendNotificationAsync(notificationRequest).ConfigureAwait(false);

            var notificationId = sendResult.GetValueOrThrow().Id;
            notificationId.Should().NotBeEmpty();
        }

        public static readonly SendNotificationRequest[] IncorrectNotifications =
        {
            null!,
            new AndroidSendNotificationRequest(null!, "MessageA", "title1"),
            new AndroidSendNotificationRequest(new('x', 60), "MessageB", "title2"),
            new AndroidSendNotificationRequest("token", new('y', 2100), "title3"),
            new AndroidSendNotificationRequest("token", "MessageC", new('z', 300)),
            new IosSendNotificationRequest(null!, "alert1"),
            new IosSendNotificationRequest(new('x', 60), "alert2"),
            new IosSendNotificationRequest("tokenA", new('y', 2100)),
        };

        [Test]
        public async Task Return_Bad_Request([ValueSource(nameof(IncorrectNotifications))] SendNotificationRequest notificationRequest)
        {
            var client = await this.clientFactory.Create().ConfigureAwait(false);

            var sendResult = await client.SendNotificationAsync(notificationRequest).ConfigureAwait(false);

            sendResult.GetFaultOrThrow().Should().BeBadRequest();
        }
    }
}
