using System;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public sealed class TestMethodInjectionModuleAttribute : Attribute, IFixtureServiceCollectionModule
    {
        public void ConfigureServices(IServiceCollection serviceCollection, Type type) =>
            serviceCollection.AddTestMethodInjection();
    }
}
