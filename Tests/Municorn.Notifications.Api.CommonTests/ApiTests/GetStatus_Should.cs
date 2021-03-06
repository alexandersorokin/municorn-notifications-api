using System;
using System.Threading.Tasks;
using FluentAssertions;
using Kontur.Results;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection;
using NUnit.Framework;

namespace Municorn.Notifications.Api.Tests.ApiTests
{
    [TestFixture]
    internal class GetStatus_Should : IServiceClientFixture
    {
        private static readonly SendNotificationRequest[] Notifications =
        {
            new AndroidSendNotificationRequest("token", "Message", "title"),
            new IosSendNotificationRequest("token", "alert"),
        };

        [InjectFieldDependency]
        private readonly ClientFactory clientFactory = default!;

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
