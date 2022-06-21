using FluentAssertions;
using Municorn.Notifications.Api.Tests.DependencyInjection.AutoMethods;
using Municorn.Notifications.Api.Tests.DependencyInjection.ScopeAsyncLocal;
using NUnit.Framework;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.BeforeFixtureConstructor.Tests
{
    [TestFixtureInjectable]
    [TestModule]
    [PrimaryConstructor]
    internal partial class Inject_AsyncLocal_Should
    {
        private readonly AsyncLocalTestCaseServiceResolver<IFixtureSetUp> service;

        [Test]
        [Repeat(2)]
        public void Case()
        {
            this.service.Value.Should().NotBeNull();
        }

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(2)]
        public void Cases(int value)
        {
            this.service.Value.Should().NotBeNull();
        }
    }
}
