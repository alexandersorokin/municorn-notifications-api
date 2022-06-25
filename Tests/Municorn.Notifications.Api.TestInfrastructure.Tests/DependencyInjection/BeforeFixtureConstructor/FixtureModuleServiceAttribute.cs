using System;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.BeforeFixtureConstructor
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true)]
    public sealed class FixtureModuleServiceAttribute : Attribute, IFixtureServiceCollectionModule
    {
        private readonly Type serviceType;

        private readonly Type? implementationType;

        public FixtureModuleServiceAttribute(Type serviceType, Type? implementationType = null)
        {
            this.serviceType = serviceType;
            this.implementationType = implementationType;
        }

        public void ConfigureServices(IServiceCollection serviceCollection, ITypeInfo typeInfo) =>
            serviceCollection.AddSingleton(this.serviceType, this.implementationType ?? this.serviceType);
    }
}
