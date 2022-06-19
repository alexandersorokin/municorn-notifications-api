using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.NotificationFeature.App;
using Municorn.Notifications.Api.NotificationFeature.Data;
using Municorn.Notifications.Api.Tests.DependencyInjection;
using NUnit.Framework;

namespace Municorn.Notifications.Api.Tests.IntegrationTests
{
    [TestFixture]
    internal class IosNotificationSender_Should : IConfigureServices
    {
        [TestDependency]
        private readonly AsyncLocalTestCaseServiceResolver serviceResolver = default!;

        public void ConfigureServices(IServiceCollection serviceCollection) =>
            serviceCollection
                .RegisterWaiter()
                .AddSingleton<NotificationStatusRepository>()
                .RegisterLogSniffer()
                .RegisterMicrosoftLogger()
                .AddScoped<IosNotificationSender>();

        [Test]
        public async Task Deliver_Message()
        {
            var result = await this.CreateNotificationSender()
                .Send(new("token", "alert"))
                .ConfigureAwait(false);

            result.Delivered.Should().BeTrue();
        }

        [Test]
        public async Task Not_Deliver_Fifth_Message()
        {
            var sender = this.CreateNotificationSender();
            IosNotificationData data = new("token", "alert");

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
            var result = await this.CreateNotificationSender()
                .Send(new("token", "alert"))
                .ConfigureAwait(false);

            var status = this.serviceResolver.ResolveService<NotificationStatusRepository>().GetStatus(result.Id);

            status.HasSome.Should().BeTrue();
        }

        [Test]
        public async Task Write_Message_To_Log()
        {
            var token = Guid.NewGuid().ToString();
            var alert = Guid.NewGuid().ToString();
            const int priority = 500;
            IosNotificationData data = new(token, alert)
            {
                Priority = priority,
                IsBackground = false,
            };

            await this.CreateNotificationSender().Send(data).ConfigureAwait(false);

            var expectedMessages = new[]
            {
                token,
                alert,
                priority.ToString(CultureInfo.InvariantCulture),
                bool.FalseString,
            };
            this
                .GetLogMessageContainer()
                .GetMessages()
                .Should()
                .Contain(logMessage => expectedMessages.All(logMessage.Contains));
        }

        [Test]
        public async Task Write_Sender_Name_To_Log()
        {
            await this.CreateNotificationSender()
                .Send(new("token", "alert"))
                .ConfigureAwait(false);

            this
                .GetLogMessageContainer()
                .GetMessages()
                .Should()
                .Contain(logMessage => logMessage.Contains("IosSender"));
        }

        private LogMessageContainer GetLogMessageContainer() => this.serviceResolver.ResolveService<LogMessageContainer>();

        private IosNotificationSender CreateNotificationSender() => this.serviceResolver.ResolveService<IosNotificationSender>();
    }
}
