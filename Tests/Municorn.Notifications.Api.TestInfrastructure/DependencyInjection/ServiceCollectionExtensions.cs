using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection
{
    internal static class ServiceCollectionExtensions
    {
        internal static IServiceCollection RegisterFixtures(this IServiceCollection serviceCollection, ITest? currentTest)
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

        internal static IServiceCollection RegisterModules(this IServiceCollection serviceCollection, ITypeInfo typeInfo)
        {
            foreach (var module in typeInfo.GetCustomAttributes<IFixtureModule>(true))
            {
                module.ConfigureServices(serviceCollection, typeInfo);
            }

            return serviceCollection;
        }
    }
}
