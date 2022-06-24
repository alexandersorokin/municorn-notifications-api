using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection
{
    internal static class ServiceCollectionExtensions
    {
        internal static IServiceCollection AddFixtures(this IServiceCollection serviceCollection, ITest? currentTest)
        {
            while (currentTest != null)
            {
                if (currentTest.Fixture is { } fixture)
                {
                    serviceCollection.AddSingleton(fixture.GetType(), fixture);
                }

                currentTest = currentTest.Parent;
            }

            return serviceCollection;
        }

        internal static IServiceCollection AddFixtureModules(this IServiceCollection serviceCollection, ITypeInfo typeInfo)
        {
            var customAttributes = typeInfo
                .GetCustomAttributes<IFixtureServiceCollectionModule>(true)
                .Concat(typeInfo.Type
                    .GetInterfaces()
                    .SelectMany(interfaceType => interfaceType.GetCustomAttributes(typeof(IFixtureServiceCollectionModule), false)
                        .Cast<IFixtureServiceCollectionModule>()));
            foreach (var module in customAttributes)
            {
                module.ConfigureServices(serviceCollection, typeInfo);
            }

            return serviceCollection;
        }

        internal static IServiceCollection AddFixtureAutoMethods(this IServiceCollection serviceCollection) =>
            serviceCollection
                .AddSingleton<FixtureOneTimeSetUpRunner>()
                .AddSingleton<TestActionMethodManager>()
                .AddScoped<TestAccessor>()
                .AddScoped<FixtureSetUpRunner>();
    }
}
