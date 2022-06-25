using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Abstractions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.TestCommunication;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFieldInjection(this IServiceCollection serviceCollection, Type fixtureType)
        {
            var fields = fixtureType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

            AddServices(serviceCollection, fields);

            return serviceCollection
                .AddSingleton(new FieldInfoProvider(fields))
                .AddSingleton<IFixtureOneTimeSetUpService, SingletonFieldInitializer>();
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
