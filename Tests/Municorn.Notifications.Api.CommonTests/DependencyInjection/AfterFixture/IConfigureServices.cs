using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.Tests.DependencyInjection.Scope;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.AfterFixture
{
    [DependencyInjectionContainer]
    internal interface IConfigureServices : IUseContainerToResolveTestArguments
    {
        void ConfigureServices(IServiceCollection serviceCollection);
    }
}
