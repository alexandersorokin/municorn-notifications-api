﻿using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureActions
{
    [TestFixture]
    internal class Use_Configure_Services_Should : IWithFields
    {
        [FieldDependency]
        private readonly MockService service = default!;

        public void SetUpServices(IServiceCollection serviceCollection) => serviceCollection
            .AddSingleton<MockService>();

        [Test]
        [Repeat(2)]
        public void Case() => this.service.Should().NotBeNull();

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(2)]
        public void Cases(int value) => this.service.Should().NotBeNull();
    }
}
