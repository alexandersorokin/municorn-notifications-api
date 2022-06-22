namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Scopes.AsyncLocal
{
    [PrimaryConstructor]
    public partial class AsyncLocalTestCaseServiceResolver<TService>
        where TService : notnull
    {
        private readonly AsyncLocalServiceProvider asyncLocalServiceProvider;

        public TService Value => this.asyncLocalServiceProvider.GetRequiredService<TService>();
    }
}
