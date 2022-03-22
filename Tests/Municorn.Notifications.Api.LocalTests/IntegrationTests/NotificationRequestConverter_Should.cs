﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.NotificationFeature.App;
using Municorn.Notifications.Api.NotificationFeature.Data;
using Municorn.Notifications.Api.NotificationFeature.View;
using Municorn.Notifications.Api.Tests.DependencyInjection;
using NUnit.Framework;

namespace Municorn.Notifications.Api.Tests.IntegrationTests
{
    [TestFixture]
    [DependencyInjectionContainer]
    internal class NotificationRequestConverter_Should : IConfigureServices
    {
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection
                .RegisterWaiter()
                .AddSingleton<NotificationStatusRepository>()
                .RegisterLogSniffer()
                .RegisterMicrosoftLogger()
                .AddScoped<INotificationSender<AndroidNotificationData>, AndroidNotificationSender>()
                .AddScoped<INotificationSender<IosNotificationData>, IosNotificationSender>()
                .AddScoped<NotificationRequestConverter>();
        }

        private static IEnumerable<TestCaseData> Cases
        {
            get
            {
                var deviceToken = Guid.NewGuid().ToString();
                var message = Guid.NewGuid().ToString();
                var title = Guid.NewGuid().ToString();
                var condition = Guid.NewGuid().ToString();
                AndroidSendNotificationRequest androidRequest = new(deviceToken, message, title)
                {
                    Condition = condition,
                };
                var androidExpectedMessages = new[]
                {
                    deviceToken,
                    message,
                    title,
                    condition,
                };

                yield return new(androidRequest, androidExpectedMessages);

                var pushToken = Guid.NewGuid().ToString();
                var alert = Guid.NewGuid().ToString();
                const int priority = 333322;
                IosSendNotificationRequest iosRequest = new(pushToken, alert)
                {
                    Priority = priority,
                    IsBackground = true,
                };
                var iosExpectedMessages = new[]
                {
                    pushToken,
                    alert,
                    priority.ToString(CultureInfo.InvariantCulture),
                    bool.TrueString,
                };

                yield return new(iosRequest, iosExpectedMessages);
            }
        }

        [TestCaseSource(nameof(Cases))]
        public async Task Write_Message_To_Log(
            SendNotificationRequest request,
            IEnumerable<string> expectedMessages)
        {
            var notification = request.Accept(this.ResolveService<NotificationRequestConverter>());

            await notification.Send().ConfigureAwait(false);
            this
                .ResolveService<LogMessageContainer>()
                .GetMessages()
                .Should()
                .Contain(logMessage => expectedMessages.All(logMessage.Contains));
        }
    }
}
