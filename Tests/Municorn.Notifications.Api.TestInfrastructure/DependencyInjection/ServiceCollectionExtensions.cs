using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AutoMethods;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Scopes;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Scopes.AsyncLocal;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Scopes.Inject;
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
                .GetCustomAttributes<IFixtureModule>(true)
                .Concat(typeInfo.Type
                    .GetInterfaces()
                    .SelectMany(interfaceType => interfaceType.GetCustomAttributes(typeof(IFixtureModule), true)
                        .Cast<IFixtureModule>()))
                .Distinct();
            foreach (var module in customAttributes)
            {
                module.ConfigureServices(serviceCollection, typeInfo);
            }

            return serviceCollection;
        }

        internal static IServiceCollection AddAsyncLocal(this IServiceCollection serviceCollection) =>
            serviceCollection
                .AddSingleton(sp => new AsyncLocalServiceProvider(sp.GetRequiredService<IFixtureProvider>()))
                .AddSingleton(typeof(AsyncLocalServiceProvider<>));

        internal static IServiceCollection AddFixtureAutoMethods(this IServiceCollection serviceCollection) =>
            serviceCollection
                .AddSingleton<FixtureOneTimeSetUpRunner>()
                .AddSingleton<IFixtureOneTimeSetUp, MapProviderSaver>();

        internal static IServiceCollection AddTestActionManager(this IServiceCollection serviceCollection) =>
            serviceCollection
                .AddSingleton<TestActionMethodManager>()
                .AddScoped<TestAccessor>()
                .AddScoped<FixtureSetUpRunner>()
                .AddScoped<IFixtureSetUp, MethodPatcher>()
                .AddScoped<IFixtureSetUp, ScopeSaver>();
    }
}
