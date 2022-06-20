using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.NotificationFeature.App;
using Municorn.Notifications.Api.NotificationFeature.Data;
using Municorn.Notifications.Api.Tests.DependencyInjection.AfterFixtureConstructor;
using Municorn.Notifications.Api.Tests.DependencyInjection.ScopeAsyncLocal;
using NUnit.Framework;

namespace Municorn.Notifications.Api.Tests.IntegrationTests
{
    [TestFixture]
    internal class AndroidNotificationSender_Should : IConfigureServices
    {
        [TestDependency]
        private readonly AsyncLocalTestCaseServiceResolver<AndroidNotificationSender> androidNotificationSender = default!;

        [TestDependency]
        private readonly AsyncLocalTestCaseServiceResolver<LogMessageContainer> logMessageContainer = default!;

        [TestDependency]
        private readonly AsyncLocalTestCaseServiceResolver<NotificationStatusRepository> notificationStatusRepository = default!;

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
        public async Task Save_Status_To_Repository()
        {
            var result = await this.androidNotificationSender.Value
                .Send(new("token", "message", "title"))
                .ConfigureAwait(false);

            var status = this.notificationStatusRepository.Value.GetStatus(result.Id);

            status.HasSome.Should().BeTrue();
        }

        [Test]
        public async Task Write_Message_To_Log()
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
            this
                .logMessageContainer.Value
                .GetMessages()
                .Should()
                .Contain(logMessage => expectedMessages.All(logMessage.Contains));
        }

        [Test]
        public async Task Write_Sender_Name_To_Log()
        {
            await this.androidNotificationSender.Value
                .Send(new("token", "message", "title"))
                .ConfigureAwait(false);

            this
                .logMessageContainer.Value
                .GetMessages()
                .Should()
                .Contain(logMessage => logMessage.Contains("AndroidSender"));
        }
    }
}
