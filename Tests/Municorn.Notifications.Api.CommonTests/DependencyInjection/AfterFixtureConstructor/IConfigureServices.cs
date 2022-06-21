using Microsoft.Extensions.DependencyInjection;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.AfterFixtureConstructor
{
    [DependencyInjectionContainer]
    public interface IConfigureServices
    {
        void ConfigureServices(IServiceCollection serviceCollection);
    }
}
