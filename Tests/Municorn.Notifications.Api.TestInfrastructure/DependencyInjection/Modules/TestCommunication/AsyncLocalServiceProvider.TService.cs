using Microsoft.Extensions.DependencyInjection;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.TestCommunication
{
    [PrimaryConstructor]
    internal sealed partial class AsyncLocalServiceProvider<TService> : IAsyncLocalServiceProvider<TService>
        where TService : notnull
    {
        private readonly IAsyncLocalServiceProvider asyncLocalServiceProvider;

        public TService Value => this.asyncLocalServiceProvider.GetRequiredService<TService>();
    }
}
