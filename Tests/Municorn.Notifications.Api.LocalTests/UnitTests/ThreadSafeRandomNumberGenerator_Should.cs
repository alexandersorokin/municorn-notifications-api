using FluentAssertions;
using Municorn.Notifications.Api.NotificationFeature.App;
using NUnit.Framework;

namespace Municorn.Notifications.Api.Tests.UnitTests
{
    [TestFixture]
    internal class ThreadSafeRandomNumberGenerator_Should
    {
        [Test]
        [Repeat(10)]
        public void Generate_Random_Number()
        {
            using ThreadSafeRandomNumberGenerator generator = new();

            var number = generator.Next(3);

            number.Should().BeInRange(0, 3);
        }
    }
}
