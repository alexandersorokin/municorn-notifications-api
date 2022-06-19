using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.Tests.DependencyInjection
{
    [PrimaryConstructor]
    internal partial class TestCaseServiceResolver
    {
        private readonly TestCaseServiceScopeMap testCaseServiceScopeMap;

        internal TService ResolveService<TService>()
            where TService : notnull
        {
            return this.testCaseServiceScopeMap.ResolveService<TService>(TestExecutionContext.CurrentContext.CurrentTest);
        }
    }
}
