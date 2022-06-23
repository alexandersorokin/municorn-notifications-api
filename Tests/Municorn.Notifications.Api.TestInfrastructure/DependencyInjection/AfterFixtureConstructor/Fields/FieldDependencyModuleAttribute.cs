using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor.Fields
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public sealed class FieldDependencyModuleAttribute : Attribute, IFixtureModule
    {
        public void ConfigureServices(IServiceCollection serviceCollection, ITypeInfo typeInfo)
        {
            var fields = typeInfo.Type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

            var serviceTypes =
                from field in fields
                from attribute in field.GetCustomAttributes<RegisterDependencyAttribute>()
                select (field.FieldType, attribute);

            foreach (var (serviceType, attribute) in serviceTypes)
            {
                serviceCollection.AddSingleton(serviceType, attribute.ImplementationType ?? serviceType);
            }

            serviceCollection
                .AddSingleton(new FieldInfoProvider(fields))
                .AddSingleton<IFixtureOneTimeSetUp, SingletonFieldInitializer>();
        }
    }
}
