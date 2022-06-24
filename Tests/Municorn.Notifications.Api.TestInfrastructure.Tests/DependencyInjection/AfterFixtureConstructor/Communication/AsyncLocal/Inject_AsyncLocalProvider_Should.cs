﻿using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.TestCommunication;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.TestCommunication.AsyncLocal;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.AfterFixtureConstructor.Communication.AsyncLocal
{
    [TestFixture]
    [TestCommunicationModule]
    internal class Inject_AsyncLocalProvider_Should : IWithFields
    {
        [FieldDependency]
        private readonly IAsyncLocalServiceProvider provider = default!;

        public void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection
            .AddScoped<ILog, SilentLog>();

        [SetUp]
        public void SetUp()
        {
            this.provider.GetRequiredService<ILog>().Should().NotBeNull();
        }

        [TearDown]
        public void TearDown()
        {
            this.provider.GetRequiredService<ILog>().Should().NotBeNull();
        }

        [Test]
        [Repeat(2)]
        public void Case()
        {
            this.provider.GetRequiredService<ILog>().Should().NotBeNull();
        }

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(2)]
        public void Cases(int value)
        {
            this.provider.GetRequiredService<ILog>().Should().NotBeNull();
        }
    }
}
