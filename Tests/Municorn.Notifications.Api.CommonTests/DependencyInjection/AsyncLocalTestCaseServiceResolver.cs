﻿using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.Tests.DependencyInjection
{
    [PrimaryConstructor]
    internal partial class AsyncLocalTestCaseServiceResolver
    {
        private readonly TestServiceProviderMap testServiceProviderMap;

        internal TService ResolveService<TService>()
            where TService : notnull =>
            this.testServiceProviderMap
                .GetScope(TestExecutionContext.CurrentContext.CurrentTest)
                .ServiceProvider.GetRequiredService<TService>();
    }
}
