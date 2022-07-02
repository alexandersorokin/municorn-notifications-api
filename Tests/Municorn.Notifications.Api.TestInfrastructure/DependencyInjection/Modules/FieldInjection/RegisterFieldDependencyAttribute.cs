using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field)]
    public sealed class RegisterFieldDependencyAttribute : Attribute, IFieldServiceCollectionModule
    {
        public RegisterFieldDependencyAttribute(Type implementationType) => this.ImplementationType = implementationType;

        public RegisterFieldDependencyAttribute()
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
