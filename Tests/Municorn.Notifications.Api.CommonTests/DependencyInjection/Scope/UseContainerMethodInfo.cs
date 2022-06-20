using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.Scope
{
    internal class UseContainerMethodInfo : IMethodInfo
    {
        private readonly IMethodInfo implementation;
        private readonly FixtureServiceProviderMap fixtureServiceProviderMap;

        public UseContainerMethodInfo(IMethodInfo implementation, FixtureServiceProviderMap fixtureServiceProviderMap)
        {
            this.implementation = implementation;
            this.fixtureServiceProviderMap = fixtureServiceProviderMap;
        }

        public T[] GetCustomAttributes<T>(bool inherit)
            where T : class =>
            this.implementation.GetCustomAttributes<T>(inherit);

        public bool IsDefined<T>(bool inherit)
            where T : class =>
            this.implementation.IsDefined<T>(inherit);

        public IParameterInfo[] GetParameters() => this.implementation.GetParameters();

        public Type[] GetGenericArguments() => this.implementation.GetGenericArguments();

        public IMethodInfo MakeGenericMethod(params Type[] typeArguments) => this.implementation.MakeGenericMethod(typeArguments);

        public object? Invoke(object? fixture, params object?[]? args)
        {
            Lazy<IServiceProvider> serviceProvider = new(() => this.GetServiceProvider(fixture));
            args = this.ResolveArgs(serviceProvider, args ?? Array.Empty<object?>()).ToArray();
            return this.implementation.Invoke(fixture, args);
        }

        private IServiceProvider GetServiceProvider(object? fixture) =>
            fixture is not null && this.fixtureServiceProviderMap.TryGetScope(fixture, out var serviceScope)
                ? serviceScope.ServiceProvider
                : throw new InvalidOperationException($"Failed to resolve services for {this.MethodInfo.Name}. No service provider is registered for fixture {fixture}.");

        private IEnumerable<object?> ResolveArgs(Lazy<IServiceProvider> serviceProvider, IReadOnlyList<object?> args)
        {
            var parameters = this.implementation.GetParameters();
            var usedIndex = 0;
            foreach (var parameter in parameters)
            {
                if (usedIndex < args.Count)
                {
                    var resolveArgs = args[usedIndex++];
                    var type = resolveArgs?.GetType();
                    if (type is not null && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(InjectedService<>))
                    {
                        yield return serviceProvider.Value.GetRequiredService(type.GetGenericArguments().Single());
                    }
                    else
                    {
                        yield return resolveArgs;
                    }
                }
                else if (parameter.IsOptional)
                {
                    yield return Type.Missing;
                }
            }
        }

        public ITypeInfo TypeInfo => this.implementation.TypeInfo;

        public MethodInfo MethodInfo => this.implementation.MethodInfo;

        public string Name => this.implementation.Name;

        public bool IsAbstract => this.implementation.IsAbstract;

        public bool IsPublic => this.implementation.IsPublic;

        public bool IsStatic => this.implementation.IsStatic;

        public bool ContainsGenericParameters => this.implementation.ContainsGenericParameters;

        public bool IsGenericMethod => this.implementation.IsGenericMethod;

        public bool IsGenericMethodDefinition => this.implementation.IsGenericMethodDefinition;

        public ITypeInfo ReturnType => this.implementation.ReturnType;
    }
}