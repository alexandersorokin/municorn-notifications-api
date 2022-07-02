using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFieldInjection(this IServiceCollection serviceCollection, Type fixtureType)
        {
            var fields = fixtureType.GetBaseTypes()
                .Append(fixtureType)
                .SelectMany(type => type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
                .ToArray();

            AddServices(serviceCollection, fields);

            return serviceCollection
                .AddSingleton(new FieldInfoProvider(fields))
                .AddSingleton<IFixtureOneTimeSetUpService, SingletonFieldInitializer>();
        }

        private static IEnumerable<Type> GetBaseTypes(this Type type)
        {
            while (type.BaseType != null)
            {
                yield return type = type.BaseType;
            }
        }

        private static void AddServices(IServiceCollection serviceCollection, IEnumerable<FieldInfo> fields)
        {
            var modules =
                from field in fields
                from module in field.GetAttributes<IFieldServiceCollectionModule>(false)
                select (field, module);

            foreach (var (field, module) in modules)
            {
                module.ConfigureServices(serviceCollection, field);
            }
        }
    }
}
