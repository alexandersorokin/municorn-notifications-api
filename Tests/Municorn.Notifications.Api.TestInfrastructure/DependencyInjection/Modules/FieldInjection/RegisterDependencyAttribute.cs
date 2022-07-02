using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.TestCommunication;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field)]
    public sealed class RegisterDependencyAttribute : Attribute, IFieldServiceCollectionModule
    {
        public RegisterDependencyAttribute(Type implementationType) => this.ImplementationType = implementationType;

        public RegisterDependencyAttribute()
        {
        }

        public Type? ImplementationType { get; }

        public void ConfigureServices(IServiceCollection serviceCollection, FieldInfo fieldInfo)
        {
            var serviceType = fieldInfo.FieldType;

            if (serviceType.IsConstructedGenericType &&
                serviceType.GetGenericTypeDefinition() == typeof(IAsyncLocalServiceProvider<>))
            {
                var genericArgument = serviceType.GenericTypeArguments.Single();
                serviceCollection.AddScoped(genericArgument, this.ImplementationType ?? genericArgument);
            }
            else
            {
                serviceCollection.AddSingleton(serviceType, this.ImplementationType ?? serviceType);
            }
        }
    }
}
