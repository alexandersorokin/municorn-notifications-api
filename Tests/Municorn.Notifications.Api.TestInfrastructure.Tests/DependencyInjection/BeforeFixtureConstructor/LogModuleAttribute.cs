using System;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Abstractions;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.BeforeFixtureConstructor
{
    [AttributeUsage(AttributeTargets.Class)]
    internal sealed class LogModuleAttribute : Attribute, IFixtureServiceCollectionModule
    {
        public void ConfigureServices(IServiceCollection serviceCollection, ITypeInfo typeInfo) =>
            serviceCollection.AddContextualLog();
    }
}
