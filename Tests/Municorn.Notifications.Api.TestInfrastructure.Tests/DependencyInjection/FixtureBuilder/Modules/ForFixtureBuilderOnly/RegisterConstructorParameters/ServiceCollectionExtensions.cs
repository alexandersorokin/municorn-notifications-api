using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureBuilder.Modules.ForFixtureBuilderOnly.RegisterConstructorParameters
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRegisterConstructorParameters(this IServiceCollection serviceCollection, Type fixtureType)
        {
            var serviceTypes =
                from constructor in fixtureType.GetConstructors()
                from parameter in constructor.GetParameters()
                from attribute in parameter.GetCustomAttributes<RegisterDependencyAttribute>(false)
                select (parameter.ParameterType, attribute);

            foreach (var (serviceType, attribute) in serviceTypes)
            {
                serviceCollection.AddSingleton(serviceType, attribute.ImplementationType ?? serviceType);
            }

            return serviceCollection;
        }
    }
}
