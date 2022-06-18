using System;
using System.Threading.Tasks;
using FluentAssertions;
using Kontur.Results;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.Tests.DependencyInjection;
using NUnit.Framework;

namespace Municorn.Notifications.Api.Tests.ApiTests
{
    [TestFixture]
    internal class GetStatus_Should : IConfigureServices
    {
        [TestDependency]
        private readonly ClientFactory clientFactory = default!;

        public void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection.RegisterClientFactory();

        public static readonly SendNotificationRequest[] Notifications =
        {
            new AndroidSendNotificationRequest("token", "Message", "title"),
            new IosSendNotificationRequest("token", "alert"),
        };

        [Test]
        public async Task Succeed([ValueSource(nameof(Notifications))] SendNotificationRequest notificationRequest)
        {
            var client = await this.clientFactory.Create().ConfigureAwait(false);
            var sendResult = await client.SendNotificationAsync(notificationRequest).ConfigureAwait(false);
            var notificationId = sendResult.GetValueOrThrow().Id;

            var status = await client.GetNotificationStatusAsync(notificationId).ConfigureAwait(false);

            status.GetValueOrThrow().Status.Should().BeOneOf(SendStatus.Delivered, SendStatus.NotDelivered);
        }

        [Test]
        public async Task Return_Bad_Request()
        {
            var client = await this.clientFactory.Create().ConfigureAwait(false);

            var status = await client.GetNotificationStatusAsync(Guid.Empty).ConfigureAwait(false);

            status.GetFaultOrThrow().Should().BeBadRequest();
        }

        [Test]
        public async Task Return_Not_Found()
        {
            var client = await this.clientFactory.Create().ConfigureAwait(false);

            var status = await client.GetNotificationStatusAsync(Guid.NewGuid()).ConfigureAwait(false);

            status.GetFaultOrThrow().Should().HaveStatusCode(404);
        }
    }
}
