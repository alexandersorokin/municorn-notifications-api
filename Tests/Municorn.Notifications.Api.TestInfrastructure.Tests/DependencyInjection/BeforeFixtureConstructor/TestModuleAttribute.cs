using System;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AutoMethods;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.BeforeFixtureConstructor;
using Municorn.Notifications.Api.TestInfrastructure.Logging;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.BeforeFixtureConstructor
{
    [AttributeUsage(AttributeTargets.Class)]
    internal sealed class TestModuleAttribute : Attribute, IModule
    {
        public void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection
            .AddContextualLog()
            .AddSingleton<Counter>()
            .AddSingleton<IFixtureOneTimeSetUp, FixtureTimeLogger>()
            .AddScoped<IFixtureSetUp, TestTimeLogger>();
    }
}
