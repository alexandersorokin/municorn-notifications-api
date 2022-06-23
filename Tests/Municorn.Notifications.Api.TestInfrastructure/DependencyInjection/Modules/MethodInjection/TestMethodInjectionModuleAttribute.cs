using System;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public sealed class TestMethodInjectionModuleAttribute : Attribute, IFixtureModule
    {
        public void ConfigureServices(IServiceCollection serviceCollection, ITypeInfo typeInfo) =>
            serviceCollection
                .AddScoped<UseContainerMethodInfoFactory>()
                .AddScoped<IFixtureSetUp, UseContainerMethodInfoPatcher>();
    }
}
