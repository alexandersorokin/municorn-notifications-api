﻿using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.AfterFixtureConstructor.ImplicitInterface
{
    internal interface IWithNoServices : IFixtureServiceProvider
    {
        void IFixtureServiceProvider.ConfigureServices(IServiceCollection serviceCollection)
        {
            // Nothing by default
        }
    }
}
