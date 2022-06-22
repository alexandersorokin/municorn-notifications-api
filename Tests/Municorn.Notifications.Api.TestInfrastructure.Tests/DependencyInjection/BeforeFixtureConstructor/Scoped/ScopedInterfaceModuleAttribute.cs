using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.BeforeFixtureConstructor.Scoped
{
    [AttributeUsage(AttributeTargets.Class)]
    internal sealed class ScopedInterfaceModuleAttribute : Attribute, IFixtureModule
    {
        public void ConfigureServices(IServiceCollection serviceCollection, ITypeInfo typeInfo)
        {
            serviceCollection.AddScoped(typeof(ScopedProvider<>));

            var scopedTypes = typeInfo.Type.GetInterfaces()
                .Where(i => i.IsConstructedGenericType)
                .Where(i => i.GetGenericTypeDefinition() == typeof(IScoped<>))
                .Select(i => i.GetGenericArguments().Single());

            foreach (var scopedType in scopedTypes)
            {
                var genericType = typeof(ScopedProvider<>).MakeGenericType(scopedType);
                serviceCollection
                    .AddScoped(scopedType, sp =>
                    {
                        dynamic provider = sp.GetRequiredService(genericType);
                        return provider.Get();
                    });
            }
        }

        private class ScopedProvider<TService>
        {
            private readonly ITest test;

            public ScopedProvider(ITest test)
            {
                this.test = test;
            }

            public TService Get()
            {
                var fixture = (this.test.Fixture as IScoped<TService>)
                              ?? throw new InvalidOperationException($"Fixture do not implement {typeof(IScoped<TService>).FullName}");
                return fixture.Get();
            }
        }
    }
}
