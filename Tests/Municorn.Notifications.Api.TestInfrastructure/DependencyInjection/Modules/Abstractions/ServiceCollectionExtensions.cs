using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Abstractions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFixtureProvider(this IServiceCollection serviceCollection, object fixture) =>
            serviceCollection.AddSingleton<IFixtureProvider>(new FixtureProvider(fixture));

        public static IServiceCollection AddFixtureServiceCollectionModuleAttributes(this IServiceCollection serviceCollection, Type type) =>
            serviceCollection.AddFixtureServiceCollectionModuleAttributes(new TypeWrapper(type));

        internal static IServiceCollection AddFixtureServiceCollectionModuleAttributes(this IServiceCollection serviceCollection, ITypeInfo typeInfo)
        {
            var type = typeInfo.Type;
            var customAttributes = type
                .GetInterfaces()
                .SelectMany(interfaceType => interfaceType
                    .GetCustomAttributes(typeof(IFixtureServiceCollectionModule), false)
                    .Cast<IFixtureServiceCollectionModule>())
                .Concat(typeInfo.GetCustomAttributes<IFixtureServiceCollectionModule>(true));
            foreach (var module in customAttributes)
            {
                module.ConfigureServices(serviceCollection, type);
            }

            return serviceCollection;
        }
    }
}
