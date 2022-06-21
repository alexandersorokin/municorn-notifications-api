﻿namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.ScopeAsyncLocal
{
    public static class FixtureExtensions
    {
        public static TService GetRequiredService<TService>(this object fixture)
            where TService : notnull =>
            new AsyncLocalTestCaseServiceResolver(new FixtureProvider(fixture)).GetRequiredService<TService>();
    }
}