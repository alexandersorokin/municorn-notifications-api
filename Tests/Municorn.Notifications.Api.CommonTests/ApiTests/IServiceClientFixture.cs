using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection;
using Municorn.Notifications.Api.TestInfrastructure.Logging;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.Tests.ApiTests
{
    [FieldInjectionModule]
    internal interface IServiceClientFixture : IFixtureServiceProviderFramework
    {
        void IFixtureServiceProviderFramework.ConfigureServices(IServiceCollection serviceCollection) => serviceCollection
            .AddFieldInjection(this)
            .AddSingleton<ITextWriterProvider, NUnitAsyncLocalTextWriterProvider>()
            .AddSingleton<ILog, TextWriterLog>()
            .AddClientTopologyFactory()
            .AddSingleton<ClientFactory>();
    }
}
