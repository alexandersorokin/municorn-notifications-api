using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.Tests.DependencyInjection.Scope;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.AfterFixture
{
    [DependencyInjectionContainer]
    [AlternativePatcher]
    internal interface IConfigureServices
    {
        void ConfigureServices(IServiceCollection serviceCollection);
    }
}
