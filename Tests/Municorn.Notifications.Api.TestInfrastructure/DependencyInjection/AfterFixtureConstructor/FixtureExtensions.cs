﻿using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.TestCommunication;
using AsyncLocalServiceProvider = Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.TestCommunication.AsyncLocalServiceProvider;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor
{
    public static class FixtureExtensions
    {
        public static TService GetRequiredService<TService>(this IFixtureServiceProviderFramework fixture)
            where TService : notnull =>
            new AsyncLocalServiceProvider<TService>(new AsyncLocalServiceProvider(new FixtureProvider(fixture))).Value;
    }
}