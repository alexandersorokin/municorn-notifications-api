using System;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Communication.AsyncLocal;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Communication
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public sealed class TestCommunicationModuleAttribute : Attribute, IFixtureModule
    {
        public void ConfigureServices(IServiceCollection serviceCollection, ITypeInfo typeInfo) =>
            serviceCollection
                .AddSingleton(sp => new AsyncLocalServiceProvider(sp.GetRequiredService<IFixtureProvider>()))
                .AddSingleton(typeof(AsyncLocalServiceProvider<>))
                .AddSingleton<IFixtureOneTimeSetUp, MapProviderSaver>()
                .AddScoped<IFixtureSetUp, MapScopeSaver>();
    }
}
