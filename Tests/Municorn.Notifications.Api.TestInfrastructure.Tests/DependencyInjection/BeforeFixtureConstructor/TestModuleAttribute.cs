using System;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.Tests.DependencyInjection.AutoMethods;
using Municorn.Notifications.Api.Tests.Logging;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.BeforeFixtureConstructor.Tests
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
