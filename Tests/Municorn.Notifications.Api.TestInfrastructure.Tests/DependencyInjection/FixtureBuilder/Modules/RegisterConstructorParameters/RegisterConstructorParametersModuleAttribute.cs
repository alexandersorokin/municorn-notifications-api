using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureBuilder.Modules.RegisterConstructorParameters
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class RegisterConstructorParametersModuleAttribute : Attribute, IFixtureServiceCollectionModule
    {
        public void ConfigureServices(IServiceCollection serviceCollection, Type type)
        {
            var serviceTypes =
                from constructor in type.GetConstructors()
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
