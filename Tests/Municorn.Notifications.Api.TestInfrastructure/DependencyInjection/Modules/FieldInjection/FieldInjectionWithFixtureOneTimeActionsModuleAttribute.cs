using System;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Abstractions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FixtureOneTimeActions;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public sealed class FieldInjectionWithFixtureOneTimeActionsModuleAttribute : Attribute, IFixtureServiceCollectionModule
    {
        public void ConfigureServices(IServiceCollection serviceCollection, ITypeInfo typeInfo) =>
            serviceCollection
                .AddFieldInjection(typeInfo.Type)
                .AddFixtureOneTimeActions();
    }
}
