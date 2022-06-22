using System;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.BeforeFixtureConstructor
{
    [AttributeUsage(AttributeTargets.Class)]
    internal sealed class TestTimeLoggerModuleAttribute : Attribute, IFixtureModule
    {
        private static readonly LogModuleAttribute LogModule = new();

        public void ConfigureServices(IServiceCollection serviceCollection, ITypeInfo typeInfo) =>
            LogModule.ConfigureServices(serviceCollection.AddTestTimeLogger(), typeInfo);
    }
}
