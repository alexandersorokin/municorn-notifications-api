using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.BeforeFixtureConstructor
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class RegisterConstructorParametersModuleAttribute : Attribute, IFixtureModule
    {
        public void ConfigureServices(IServiceCollection serviceCollection, ITypeInfo typeInfo)
        {
            var serviceTypes =
                from constructor in typeInfo.Type.GetConstructors()
                from parameter in constructor.GetParameters()
                from attribute in parameter.GetCustomAttributes<RegisterDependencyAttribute>(false)
                select (parameter.ParameterType, attribute);

            foreach (var (serviceType, attribute) in serviceTypes)
            {
                serviceCollection.AddSingleton(serviceType, attribute.ImplementationType ?? serviceType);
            }
        }
    }
}
