﻿using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureActions.ImplicitInterface
{
    [TestFixture]
    internal class Override_ConfigureServices_Should : IWithFieldInjectionServices
    {
        public void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection
            .AddTestMethodInjection()
            .AddSingleton<Counter>();

        [Test]
        [Repeat(2)]
        public void Case([InjectDependency] Counter service) => service.Should().NotBeNull();
    }
}