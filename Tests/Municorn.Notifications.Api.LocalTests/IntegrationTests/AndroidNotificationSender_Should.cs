using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.NotificationFeature.App;
using Municorn.Notifications.Api.NotificationFeature.Data;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Scopes.AsyncLocal;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Scopes.Inject;
using NUnit.Framework;

namespace Municorn.Notifications.Api.Tests.IntegrationTests
{
    [TestFixture]
    internal class AndroidNotificationSender_Should : ITestFixture
    {
        [TestDependency]
        private readonly AsyncLocalServiceProvider<AndroidNotificationSender> androidNotificationSender = default!;

        public void ConfigureServices(IServiceCollection serviceCollection) =>
            serviceCollection
                .RegisterWaiter()
                .AddSingleton<NotificationStatusRepository>()
                .RegisterLogSniffer()
                .RegisterMicrosoftLogger()
                .AddScoped<AndroidNotificationSender>();

        [Test]
        public async Task Deliver_Message()
        {
            var result = await this.androidNotificationSender.Value
                .Send(new("token", "message", "title"))
                .ConfigureAwait(false);

            result.Delivered.Should().BeTrue();
        }

        [Test]
        public async Task Not_Deliver_Fifth_Message()
        {
            var sender = this.androidNotificationSender.Value;
            AndroidNotificationData data = new("token", "message", "title");

            await sender.Send(data).ConfigureAwait(false);
            await sender.Send(data).ConfigureAwait(false);
            await sender.Send(data).ConfigureAwait(false);
            await sender.Send(data).ConfigureAwait(false);
            var result = await sender.Send(data).ConfigureAwait(false);

            result.Delivered.Should().BeFalse();
        }

        [Test]
        public async Task Save_Status_To_Repository([Inject] NotificationStatusRepository notificationStatusRepository)
        {
            var result = await this.androidNotificationSender.Value
                .Send(new("token", "message", "title"))
                .ConfigureAwait(false);

            var status = notificationStatusRepository.GetStatus(result.Id);

            status.HasSome.Should().BeTrue();
        }

        [Test]
        public async Task Write_Message_To_Log([Inject] LogMessageContainer logMessageContainer)
        {
            var token = Guid.NewGuid().ToString();
            var message = Guid.NewGuid().ToString();
            var title = Guid.NewGuid().ToString();
            var condition = Guid.NewGuid().ToString();
            AndroidNotificationData data = new(token, message, title)
            {
                Condition = condition,
            };

            await this.androidNotificationSender.Value.Send(data).ConfigureAwait(false);

            var expectedMessages = new[]
            {
                token,
                message,
                title,
                condition,
            };
            logMessageContainer
                .GetMessages()
                .Should()
                .Contain(logMessage => expectedMessages.All(logMessage.Contains));
        }

        [Test]
        public async Task Write_Sender_Name_To_Log([Inject] LogMessageContainer logMessageContainer)
        {
            await this.androidNotificationSender.Value
                .Send(new("token", "message", "title"))
                .ConfigureAwait(false);

            logMessageContainer
                .GetMessages()
                .Should()
                .Contain(logMessage => logMessage.Contains("AndroidSender"));
        }
    }
}
