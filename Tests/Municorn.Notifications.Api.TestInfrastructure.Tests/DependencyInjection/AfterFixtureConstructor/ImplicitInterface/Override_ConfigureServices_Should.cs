﻿using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.AfterFixtureConstructor.ImplicitInterface
{
    [TestFixture]
    internal class Override_ConfigureServices_Should : IWithDependencyInjection
    {
        [TestDependency]
        private readonly ILog service = default!;

        public void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection
            .AddSingleton<ILog, SilentLog>();

        [Test]
        [Repeat(2)]
        public void Case()
        {
            this.service.Should().NotBeNull();
        }
    }
}