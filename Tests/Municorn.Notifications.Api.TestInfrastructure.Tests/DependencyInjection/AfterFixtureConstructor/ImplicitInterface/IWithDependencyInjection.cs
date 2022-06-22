﻿using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.AfterFixtureConstructor.ImplicitInterface
{
    internal interface IWithDependencyInjection : ITestFixture
    {
        void ITestFixture.ConfigureServices(IServiceCollection serviceCollection)
        {
            // Nothing by default
        }
    }
}
