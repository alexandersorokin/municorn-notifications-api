using Microsoft.Extensions.DependencyInjection;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.BeforeFixture
{
    internal interface IModule
    {
        void ConfigureServices(IServiceCollection serviceCollection);
    }
}
