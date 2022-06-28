using System;
using Microsoft.Extensions.DependencyInjection;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Abstractions
{
    public interface IFixtureServiceCollectionModule
    {
        void ConfigureServices(IServiceCollection serviceCollection, Type type);
    }
}
