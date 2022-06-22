using System;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class FixtureModuleAttribute : Attribute, IFixtureModule
    {
        private readonly Type serviceType;

        private readonly Type? implementationType;

        public FixtureModuleAttribute(Type serviceType, Type? implementationType = null)
        {
            this.serviceType = serviceType;
            this.implementationType = implementationType;
        }

        public void ConfigureServices(IServiceCollection serviceCollection, ITypeInfo typeInfo) =>
            serviceCollection.AddSingleton(this.serviceType, this.implementationType ?? this.serviceType);
    }
}
