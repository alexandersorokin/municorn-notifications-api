namespace Municorn.Notifications.Api.Tests.DependencyInjection.ScopeTestMap
{
    [PrimaryConstructor]
    internal partial class AsyncLocalTestCaseServiceResolver<TService>
        where TService : notnull
    {
        private readonly AsyncLocalTestCaseServiceResolver asyncLocalTestCaseServiceResolver;

        internal TService Value => this.asyncLocalTestCaseServiceResolver.ResolveService<TService>();
    }
}
