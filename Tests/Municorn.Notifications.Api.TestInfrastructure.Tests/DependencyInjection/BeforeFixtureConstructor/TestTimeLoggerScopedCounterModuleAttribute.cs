using System;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.TestActionManagers;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.BeforeFixtureConstructor
{
    [AttributeUsage(AttributeTargets.Class)]
    internal sealed class TestTimeLoggerScopedCounterModuleAttribute : Attribute, IFixtureModule
    {
        private static readonly LogModuleAttribute LogModule = new();

        public void ConfigureServices(IServiceCollection serviceCollection, ITypeInfo typeInfo)
        {
            serviceCollection
                .AddScoped<Counter>()
                .AddScoped<IFixtureSetUp, TestTimeLogger>();
            LogModule.ConfigureServices(serviceCollection, typeInfo);
        }
    }
}
