using System;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.NotificationFeature.App;
using Municorn.Notifications.Api.Tests.DependencyInjection;
using Municorn.Notifications.Api.Tests.DependencyInjection.BeforeFixtureConstructor;
using Municorn.Notifications.Api.Tests.Logging;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.Tests.IntegrationTests
{
    [AttributeUsage(AttributeTargets.Class)]
    internal sealed class WaiterModuleAttribute : Attribute, IModule
    {
        public void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection
            .RegisterWaiter()
            .AddSingleton<ITextWriterProvider, NUnitTextWriterProvider>()
            .AddScoped<ILog, TextWriterLog>()
            .AddScoped<IFixtureSetUp, TestTimeLogger>();
    }
}
