using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Abstractions;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureBuilder.Modules.Scoped
{
    [AttributeUsage(AttributeTargets.Class)]
    internal sealed class ScopedInterfaceModuleAttribute : Attribute, IFixtureServiceCollectionModule
    {
        private static readonly IReadOnlySet<Type> SupportedInterfaces = new HashSet<Type>
        {
            typeof(IScoped<>),
            typeof(IScopedWithSp<>),
        };

        public void ConfigureServices(IServiceCollection serviceCollection, Type type)
        {
            serviceCollection.AddScoped(typeof(ScopedProvider<>));

            var scopedTypes = type
                .GetInterfaces()
                .Where(i => i.IsConstructedGenericType)
                .Where(i => SupportedInterfaces.Contains(i.GetGenericTypeDefinition()))
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
            private readonly IServiceProvider serviceProvider;

            [UsedImplicitly]
            public ScopedProvider(ITest test, IServiceProvider serviceProvider)
            {
                this.test = test;
                this.serviceProvider = serviceProvider;
            }

            [UsedImplicitly]
            internal TService Get() =>
                this.test.Fixture switch
                {
                    IScoped<TService> fixture => fixture.Get(),
                    IScopedWithSp<TService> fixture => fixture.Get(this.serviceProvider),
                    var fixture => throw new InvalidOperationException($"Fixture {fixture} do not implement any scoped interface"),
                };
        }
    }
}
