using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Communication.AsyncLocal;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Fields
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public sealed class FieldDependenciesModuleAttribute : Attribute, IFixtureModule
    {
        public void ConfigureServices(IServiceCollection serviceCollection, ITypeInfo typeInfo)
        {
            var fields = typeInfo.Type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

            AddServices(serviceCollection, fields);

            serviceCollection
                .AddSingleton(new FieldInfoProvider(fields))
                .AddSingleton<IFixtureOneTimeSetUp, SingletonFieldInitializer>();
        }

        private static void AddServices(IServiceCollection serviceCollection, IEnumerable<FieldInfo> fields)
        {
            var serviceTypes =
                from field in fields
                from attribute in field.GetCustomAttributes<RegisterDependencyAttribute>()
                select (field.FieldType, attribute.ImplementationType);

            foreach (var (serviceType, implementationType) in serviceTypes)
            {
                if (serviceType.IsConstructedGenericType &&
                    serviceType.GetGenericTypeDefinition() == typeof(IAsyncLocalServiceProvider<>))
                {
                    var genericArgument = serviceType.GenericTypeArguments.Single();
                    serviceCollection.AddScoped(genericArgument, implementationType ?? genericArgument);
                }
                else
                {
                    serviceCollection.AddSingleton(serviceType, implementationType ?? serviceType);
                }
            }
        }
    }
}
