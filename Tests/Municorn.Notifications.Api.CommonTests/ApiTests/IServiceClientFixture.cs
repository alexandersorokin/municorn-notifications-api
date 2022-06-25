using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureActions;
using Municorn.Notifications.Api.TestInfrastructure.Logging;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.Tests.ApiTests
{
    internal interface IServiceClientFixture : IFixtureWithServiceProviderFramework
    {
        void IFixtureWithServiceProviderFramework.ConfigureServices(IServiceCollection serviceCollection) => serviceCollection
            .AddFieldInjection(this)
            .AddSingleton<ITextWriterProvider, NUnitAsyncLocalTextWriterProvider>()
            .AddSingleton<ILog, TextWriterLog>()
            .AddClientTopologyFactory()
            .AddSingleton<ClientFactory>();
    }
}
