using System;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureBuilder
{
    [AttributeUsage(AttributeTargets.Class)]
    internal sealed class MockServiceScopedModuleAttribute : Attribute, IFixtureServiceCollectionModule
    {
        public void ConfigureServices(IServiceCollection serviceCollection, Type type) =>
            serviceCollection.AddSingleton<MockService>();
    }
}
