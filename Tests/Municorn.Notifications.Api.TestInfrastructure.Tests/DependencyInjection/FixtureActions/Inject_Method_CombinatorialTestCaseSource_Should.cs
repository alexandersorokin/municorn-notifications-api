﻿using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureActions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection;
using Municorn.Notifications.Api.TestInfrastructure.NUnitAttributes;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureActions
{
    [TestFixture]
    internal class Inject_Method_CombinatorialTestCaseSource_Should : IFixtureWithServiceProviderFramework
    {
        public void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection
            .AddTestMethodInjection()
            .AddScoped<MockService>();

        [CombinatorialTestCaseSource(nameof(CaseValues))]
        [Repeat(2)]
        public void Cases(int value, [InjectDependency] MockService service) => service.Should().NotBeNull();

        private static readonly TestCaseData[] CaseValues =
        {
            new(10),
            new(11),
        };
    }
}
