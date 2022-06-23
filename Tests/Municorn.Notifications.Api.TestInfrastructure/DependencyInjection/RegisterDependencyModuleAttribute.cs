using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class RegisterDependencyModuleAttribute : Attribute, IFixtureModule
    {
        public void ConfigureServices(IServiceCollection serviceCollection, ITypeInfo typeInfo)
        {
            var services =
                from constructor in typeInfo.Type.GetConstructors()
                from parameter in constructor.GetParameters()
                from attribute in parameter.GetCustomAttributes<RegisterDependencyAttribute>(false)
                select (parameter.ParameterType, attribute.ImplementationType);
            foreach (var (parameterType, implementationType) in services)
            {
                serviceCollection.AddSingleton(parameterType, implementationType ?? parameterType);
            }
        }
    }
}
