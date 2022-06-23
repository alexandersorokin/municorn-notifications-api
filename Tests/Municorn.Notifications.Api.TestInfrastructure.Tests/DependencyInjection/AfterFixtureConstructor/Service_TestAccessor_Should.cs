﻿using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Scopes;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Scopes.AsyncLocal;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.AfterFixtureConstructor
{
    [TestFixture]
    internal class Service_TestAccessor_Should : ITestFixture
    {
        public void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection.AddSingleton<object>();

        [Test]
        [Repeat(2)]
        public void Case([InjectDependency] TestAccessor testAccessor)
        {
            testAccessor.Test.Should().Be(TestExecutionContext.CurrentContext.CurrentTest);
        }

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(2)]
        public void Cases(int value)
        {
            this.GetRequiredService<TestAccessor>().Test.Should().Be(TestExecutionContext.CurrentContext.CurrentTest);
        }
    }
}
