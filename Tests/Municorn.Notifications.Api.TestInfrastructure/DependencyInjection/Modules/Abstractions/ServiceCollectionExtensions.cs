using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Abstractions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFixtureProvider(this IServiceCollection serviceCollection, object fixture) =>
            serviceCollection.AddSingleton<IFixtureProvider>(new FixtureProvider(fixture));

        public static IServiceCollection AddFixtureServiceCollectionModuleAttributes(this IServiceCollection serviceCollection, Type type)
        {
            var customAttributes = type
                .GetInterfaces()
                .SelectMany(interfaceType => interfaceType.GetAttributes<IFixtureServiceCollectionModule>(false))
                .Concat(type.GetAttributes<IFixtureServiceCollectionModule>(true));
            foreach (var module in customAttributes)
            {
                module.ConfigureServices(serviceCollection, type);
            }

            return serviceCollection;
        }

        internal static IEnumerable<T> GetAttributes<T>(this ICustomAttributeProvider attributeProvider, bool inherit)
            where T : class => attributeProvider.GetCustomAttributes(typeof(T), inherit).Cast<T>();
    }
}
