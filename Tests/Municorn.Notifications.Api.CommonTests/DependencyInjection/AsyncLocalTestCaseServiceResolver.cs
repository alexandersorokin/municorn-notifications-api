using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.Tests.DependencyInjection
{
    [PrimaryConstructor]
    internal partial class AsyncLocalTestCaseServiceResolver
    {
        private readonly TestCaseServiceScopeMap testCaseServiceScopeMap;

        internal TService ResolveService<TService>()
            where TService : notnull
        {
            return this.testCaseServiceScopeMap
                .GetScope(TestExecutionContext.CurrentContext.CurrentTest)
                .ServiceProvider.GetRequiredService<TService>();
        }
    }
}
