using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.TestCommunication.AsyncLocal;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFieldInjection(this IServiceCollection serviceCollection, IFixtureServiceProvider fixture) =>
            serviceCollection.AddFieldInjection(fixture.GetType());

        internal static IServiceCollection AddFieldInjection(this IServiceCollection serviceCollection, Type fixtureType)
        {
            var fields = fixtureType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

            AddServices(serviceCollection, fields);

            return serviceCollection
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
