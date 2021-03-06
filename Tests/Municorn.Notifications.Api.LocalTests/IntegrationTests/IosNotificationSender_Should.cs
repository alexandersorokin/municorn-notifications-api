using System;
using System.Globalization;
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
    internal class IosNotificationSender_Should : IFixtureWithServiceProviderFramework
    {
        [InjectFieldDependency]
        private readonly IAsyncLocalServiceProvider<IosNotificationSender> iosNotificationSender = default!;

        public void ConfigureServices(IServiceCollection serviceCollection) =>
            serviceCollection
                .AddTestCommunication()
                .AddFieldInjection(this)
                .AddTestMethodInjection()
                .AddWaiter()
                .AddSingleton<NotificationStatusRepository>()
                .AddLogSniffer()
                .AddMicrosoftLogger()
                .AddScoped<IosNotificationSender>();

        [Test]
        public async Task Deliver_Message()
        {
            var result = await this.iosNotificationSender.Value
                .Send(new("token", "alert"))
                .ConfigureAwait(false);

            result.Delivered.Should().BeTrue();
        }

        [Test]
        public async Task Not_Deliver_Fifth_Message()
        {
            var sender = this.iosNotificationSender.Value;
            IosNotificationData data = new("token", "alert");

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
            var result = await this.iosNotificationSender.Value
                .Send(new("token", "alert"))
                .ConfigureAwait(false);

            var status = notificationStatusRepository.GetStatus(result.Id);

            status.HasSome.Should().BeTrue();
        }

        [Test]
        public async Task Write_Message_To_Log([InjectParameterDependency] LogMessageContainer logMessageContainer)
        {
            var token = Guid.NewGuid().ToString();
            var alert = Guid.NewGuid().ToString();
            const int priority = 500;
            IosNotificationData data = new(token, alert)
            {
                Priority = priority,
                IsBackground = false,
            };

            await this.iosNotificationSender.Value.Send(data).ConfigureAwait(false);

            var expectedMessages = new[]
            {
                token,
                alert,
                priority.ToString(CultureInfo.InvariantCulture),
                bool.FalseString,
            };
            logMessageContainer
                .GetMessages()
                .Should()
                .Contain(logMessage => expectedMessages.All(logMessage.Contains));
        }

        [Test]
        public async Task Write_Sender_Name_To_Log([InjectParameterDependency] LogMessageContainer logMessageContainer)
        {
            await this.iosNotificationSender.Value
                .Send(new("token", "alert"))
                .ConfigureAwait(false);

            logMessageContainer
                .GetMessages()
                .Should()
                .Contain(logMessage => logMessage.Contains("IosSender"));
        }
    }
}
