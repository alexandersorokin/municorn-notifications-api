using System;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Communication.AsyncLocal;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Scopes;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Communication
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
