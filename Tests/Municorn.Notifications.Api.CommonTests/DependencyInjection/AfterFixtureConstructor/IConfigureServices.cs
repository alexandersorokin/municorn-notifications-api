using Microsoft.Extensions.DependencyInjection;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.AfterFixtureConstructor
{
    [DependencyInjectionContainer]
    internal interface IConfigureServices
    {
        void ConfigureServices(IServiceCollection serviceCollection);
    }
}
