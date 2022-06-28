﻿using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureBuilder;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureBuilder.Modules.Scoped
{
    [TestFixtureInjectable]
    [TestMethodInjectionModule]
    [ScopedInterfaceModule]
    [MockServiceScopedModule]
    internal class Use_ScopedWithSp_Interface_Should : IScopedWithSp<List<MockService>>
    {
        public List<MockService> Get(IServiceProvider serviceProvider) => new()
        {
            serviceProvider.GetRequiredService<MockService>(),
        };

        [Test]
        [Repeat(2)]
        public void Case([InjectDependency] List<MockService> service) => service.Should().HaveCount(1);

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(2)]
        public void Cases([InjectDependency] List<MockService> service, int value) => service.Should().HaveCount(1);
    }
}
