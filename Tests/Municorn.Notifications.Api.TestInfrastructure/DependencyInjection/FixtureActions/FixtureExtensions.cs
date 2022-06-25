﻿using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.TestCommunication;
using AsyncLocalServiceProvider = Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.TestCommunication.AsyncLocalServiceProvider;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureActions
{
    public static class FixtureExtensions
    {
        public static TService GetRequiredService<TService>(this IFixtureWithServiceProviderFramework fixture)
            where TService : notnull =>
            new AsyncLocalServiceProvider<TService>(new AsyncLocalServiceProvider(new Modules.Abstractions.FixtureProvider(fixture))).Value;
    }
}