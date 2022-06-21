using FluentAssertions;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.BeforeFixtureConstructor.Tests
{
    [TestFixtureInjectable]
    [TestModule]
    [PrimaryConstructor]
    internal partial class Inject_Constructor_Should
    {
        private readonly ILog service;

        [Test]
        [Repeat(2)]
        public void Case()
        {
            this.service.Should().NotBeNull();
        }

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(2)]
        public void Cases(int value)
        {
            this.service.Should().NotBeNull();
        }
    }
}
