﻿using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.BeforeFixtureConstructor;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.ScopeAsyncLocal;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.BeforeFixtureConstructor.Constructors
{
    [TestFixtureInjectable("passed", TypeArgs = new[] { typeof(int) })]
    [PrimaryConstructor]
    internal partial class Inject_TypeArgument_As_Argument_Argument_Service_Should<T>
    {
        private readonly string argument;
        private readonly AsyncLocalTestCaseServiceResolver service;

        [Test]
        public void Case()
        {
            typeof(T).Should().Be(typeof(int));
            this.argument.Should().Be("passed");
            this.service.Should().NotBeNull();
        }
    }
}