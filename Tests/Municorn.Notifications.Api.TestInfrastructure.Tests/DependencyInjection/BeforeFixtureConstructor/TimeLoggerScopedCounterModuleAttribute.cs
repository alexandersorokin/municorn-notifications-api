using System;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.BeforeFixtureConstructor
{
    [AttributeUsage(AttributeTargets.Class)]
    internal sealed class TimeLoggerScopedCounterModuleAttribute : Attribute, IFixtureServiceCollectionModule
    {
        public void ConfigureServices(IServiceCollection serviceCollection, ITypeInfo typeInfo)
        {
            serviceCollection
                .AddScoped<Counter>()
                .AddScoped<IFixtureSetUp, TestTimeLogger>();
            new LogModuleAttribute().ConfigureServices(serviceCollection, typeInfo);
        }
    }
}
