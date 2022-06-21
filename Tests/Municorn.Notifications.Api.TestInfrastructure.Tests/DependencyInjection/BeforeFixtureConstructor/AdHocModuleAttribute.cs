using System;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.BeforeFixtureConstructor;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.BeforeFixtureConstructor
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    internal sealed class AdHocModuleAttribute : Attribute, IModule
    {
        private readonly Type serviceType;

        private readonly Type? implementationType;

        public AdHocModuleAttribute(Type serviceType, Type? implementationType = null)
        {
            this.serviceType = serviceType;
            this.implementationType = implementationType;
        }

        public void ConfigureServices(IServiceCollection serviceCollection, ITypeInfo typeInfo) =>
            serviceCollection.AddSingleton(this.serviceType, this.implementationType ?? this.serviceType);
    }
}
