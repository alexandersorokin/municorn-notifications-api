using System;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Abstractions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FixtureOneTimeActions;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Combo
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public sealed class FieldInjectionWithFixtureOneTimeActionsModuleAttribute : Attribute, IFixtureServiceCollectionModule
    {
        public void ConfigureServices(IServiceCollection serviceCollection, Type type) =>
            serviceCollection
                .AddFieldInjection(type)
                .AddFixtureOneTimeActions();
    }
}
