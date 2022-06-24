using Microsoft.Extensions.DependencyInjection;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Communication.AsyncLocal
{
    [PrimaryConstructor]
    internal partial class AsyncLocalServiceProvider<TService> : IAsyncLocalServiceProvider<TService>
        where TService : notnull
    {
        private readonly IAsyncLocalServiceProvider asyncLocalServiceProvider;

        public TService Value => this.asyncLocalServiceProvider.GetRequiredService<TService>();
    }
}
