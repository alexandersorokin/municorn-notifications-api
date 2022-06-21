﻿namespace Municorn.Notifications.Api.Tests.DependencyInjection.ScopeAsyncLocal
{
    [PrimaryConstructor]
    public partial class AsyncLocalTestCaseServiceResolver<TService>
        where TService : notnull
    {
        private readonly AsyncLocalTestCaseServiceResolver asyncLocalTestCaseServiceResolver;

        public TService Value => this.asyncLocalTestCaseServiceResolver.ResolveService<TService>();
    }
}