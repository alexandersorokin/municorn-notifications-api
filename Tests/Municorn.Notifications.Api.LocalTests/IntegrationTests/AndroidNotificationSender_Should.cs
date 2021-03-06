using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.NotificationFeature.App;
using Municorn.Notifications.Api.NotificationFeature.Data;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureActions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.TestCommunication;
using NUnit.Framework;

namespace Municorn.Notifications.Api.Tests.IntegrationTests
{
    [TestFixture]
    internal class AndroidNotificationSender_Should : IFixtureWithServiceProviderFramework
    {
        [InjectFieldDependency]
        private readonly IAsyncLocalServiceProvider<AndroidNotificationSender> androidNotificationSender = default!;

        public void ConfigureServices(IServiceCollection serviceCollection) =>
            serviceCollection
                .AddTestCommunication()
                .AddFieldInjection(this)
                .AddTestMethodInjection()
                .AddWaiter()
                .AddSingleton<NotificationStatusRepository>()
                .AddLogSniffer()
                .AddMicrosoftLogger()
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
        public async Task Save_Status_To_Repository([InjectParameterDependency] NotificationStatusRepository notificationStatusRepository)
        {
            var result = await this.androidNotificationSender.Value
                .Send(new("token", "message", "title"))
                .ConfigureAwait(false);

            var status = notificationStatusRepository.GetStatus(result.Id);

            status.HasSome.Should().BeTrue();
        }

        [Test]
        public async Task Write_Message_To_Log([InjectParameterDependency] LogMessageContainer logMessageContainer)
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
        public async Task Write_Sender_Name_To_Log([InjectParameterDependency] LogMessageContainer logMessageContainer)
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
