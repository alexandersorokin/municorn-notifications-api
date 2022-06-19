﻿namespace Municorn.Notifications.Api.Tests.DependencyInjection.AfterFixture
{
    internal static class ConfigureServicesExtensions
    {
        internal static TService ResolveService<TService>(this IConfigureServices configureServices)
            where TService : notnull =>
            new AsyncLocalTestCaseServiceResolver(configureServices).ResolveService<TService>();
    }
}