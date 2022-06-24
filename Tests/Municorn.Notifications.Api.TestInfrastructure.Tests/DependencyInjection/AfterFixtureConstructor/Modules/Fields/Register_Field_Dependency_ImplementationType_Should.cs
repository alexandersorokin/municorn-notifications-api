﻿using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.AfterFixtureConstructor.Modules.Fields
{
    [TestFixture]
    [FieldInjectionModule]
    internal class Register_Field_Dependency_ImplementationType_Should : IWithoutConfigureServices
    {
        [FieldDependency]
        [RegisterDependency(typeof(SilentLog))]
        private readonly ILog service = default!;

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
