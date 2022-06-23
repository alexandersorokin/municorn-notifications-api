﻿using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.AfterFixtureConstructor.Modules
{
    [TestFixture]
    [FixtureModuleService(typeof(ILog), typeof(SilentLog))]
    internal class Register_From_TestFixtureModule_On_Class_Should : IWithoutConfigureServices
    {
        [Test]
        [Repeat(2)]
        public void Case([InjectDependency] ILog service)
        {
            service.Should().NotBeNull();
        }
    }
}