using Microsoft.Extensions.DependencyInjection;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Communication.AsyncLocal
{
    [PrimaryConstructor]
    public partial class AsyncLocalServiceProvider<TService>
        where TService : notnull
    {
        private readonly AsyncLocalServiceProvider asyncLocalServiceProvider;

        public TService Value => this.asyncLocalServiceProvider.GetRequiredService<TService>();
    }
}
