using System;
using FluentAssertions;
using Kontur.Results;
using Municorn.Notifications.Api.NotificationFeature.Data;
using NUnit.Framework;

namespace Municorn.Notifications.Api.Tests.UnitTests
{
    [TestFixture]
    internal class NotificationStatusRepository_Should
    {
        [Test]
        public void Return_None_For_Unknown_Id()
        {
            NotificationStatusRepository repository = new();

            var result = repository.GetStatus(Guid.NewGuid());

            result.Should().Be(Optional.None<NotificationStatus>());
        }

        [TestCase(NotificationStatus.Delivered)]
        [TestCase(NotificationStatus.NotDelivered)]
        public void Return_Saved_Status(NotificationStatus status)
        {
            NotificationStatusRepository repository = new();
            var notificationId = Guid.NewGuid();

            repository.SaveStatus(notificationId, status);
            var result = repository.GetStatus(notificationId);

            result.GetValueOrThrow().Should().Be(status);
        }
    }
}