﻿using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.Tests.DependencyInjection.ScopeAsyncLocal;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.AfterFixtureConstructor.Tests
{
    [TestFixture]
    internal class Inject_AsyncGenericProvider_Should : IConfigureServices
    {
        [TestDependency]
        private readonly AsyncLocalTestCaseServiceResolver<ILog> service = default!;

        public void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection
            .AddScoped<ILog, SilentLog>();

        [SetUp]
        public void SetUp()
        {
            this.service.Value.Should().NotBeNull();
        }

        [TearDown]
        public void TearDown()
        {
            this.service.Value.Should().NotBeNull();
        }

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