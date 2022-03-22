using Microsoft.Extensions.DependencyInjection;

namespace Municorn.Notifications.Api.Tests.DependencyInjection
{
    internal interface IConfigureServices
    {
        void ConfigureServices(IServiceCollection serviceCollection);
    }
}
