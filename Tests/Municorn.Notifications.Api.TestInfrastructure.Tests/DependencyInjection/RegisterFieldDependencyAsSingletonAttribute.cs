using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field)]
    public sealed class RegisterFieldDependencyAsSingletonAttribute : Attribute, IFieldServiceCollectionModule
    {
        public RegisterFieldDependencyAsSingletonAttribute(Type implementationType) => this.ImplementationType = implementationType;

        public RegisterFieldDependencyAsSingletonAttribute()
        {
        }

        public Type? ImplementationType { get; }

        public void ConfigureServices(IServiceCollection serviceCollection, FieldInfo fieldInfo)
        {
            var serviceType = fieldInfo.FieldType;
            serviceCollection.AddSingleton(serviceType, this.ImplementationType ?? serviceType);
        }
    }
}
